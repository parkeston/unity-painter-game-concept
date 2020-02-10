using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

[RequireComponent(typeof(Grid))]
public class Pathfinding : MonoBehaviour
{
	private Grid grid;
	private PathRequestManager requestManager;
	private void Awake()
	{
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}

	private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Stopwatch sw = new Stopwatch();
		sw.Start();

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		if (startNode.Walkable && targetNode.Walkable)
		{ 
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			HashSet<Node> closeSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node currentNode = openSet.RemoveFirst();
				closeSet.Add(currentNode);

				if (currentNode == targetNode)
				{
					sw.Stop();
					print("path found: " + sw.ElapsedMilliseconds + "ms");
					pathSuccess = true;
					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(currentNode))
				{
					if (!neighbour.Walkable || closeSet.Contains(neighbour))
						continue;

					int newMovementCost = currentNode.GCost + GetDistance(currentNode, neighbour);
					if (newMovementCost < neighbour.GCost || !openSet.Contains(neighbour)) //if shorter or unvisited!
					{
						neighbour.GCost = newMovementCost;
						neighbour.HCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else
							openSet.UpdateItem(neighbour);
					}
				}
			}
		}

		yield return null;
		if (pathSuccess)
			waypoints = RetracePath(startNode, targetNode);

		requestManager.FinishedProcessing(waypoints, pathSuccess);

	}

	public void StartFindPath(Vector3 pathStart, Vector3 pathEnd)
	{
		StartCoroutine(FindPath(pathStart, pathEnd));
	}

	//heruistic for diagonal movement, not using Vector3.distance cause it's Euclidean heruistic
	private int GetDistance(Node nodeA, Node nodeB)
	{
		//D = 10 - cost for moving horizontally
		//D2 = 14 - cost for moving diagonally
		// D*(dx+dy)+(D2-2*D)*min(dx,dy) - heruistic result for diagonal movement

		int dx = Mathf.Abs(nodeA.GridX - nodeB.GridX);
		int dy = Mathf.Abs(nodeA.GridY - nodeB.GridY);

		return 10 * (dx + dy) + (14 - 2 * 10) * Mathf.Min(dx, dy);
	}

	private Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while(currentNode!=startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);

		return waypoints;
	}

	private Vector3[] SimplifyPath(List<Node> path)
	{
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for(int i =1;i<path.Count;i++)
		{
			Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
			if (directionNew != directionOld)
				waypoints.Add(path[i].WorldPosition);
			directionOld = directionNew;
		}

		return waypoints.ToArray();
	}
}
