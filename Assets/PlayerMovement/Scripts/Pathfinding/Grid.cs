using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	[SerializeField] private bool displayGridGizmos;
	[SerializeField] private LayerMask unwakableArea;
	[SerializeField] private Vector2 gridWorldSize;
	[SerializeField] private float nodeRadius;

	private Node[,] grid;

	private float nodeDiameter;
	private int gridSizeX, gridSizeY;

	public int MaxSize => gridSizeX * gridSizeY;

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null && displayGridGizmos)
			foreach (Node node in grid)
			{
				Gizmos.color = node.Walkable ? Color.white : Color.red;
				Gizmos.DrawCube(node.WorldPosition, Vector3.one * (nodeDiameter - 0.1f));
			}
	}

	private void Awake()
	{
		nodeDiameter = nodeRadius * 2;

		//size of one grid cell
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

		CreateGrid();
	}

	private void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; //forward - represents y from 2d plane on 3d plane

		for(int x = 0; x<gridSizeX;x++)
			for(int y = 0; y<gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwakableArea));
				grid[x, y] = new Node(walkable, worldPoint,x,y);
			}
	}

	public Node NodeFromWorldPoint(Vector3 worldPosition)
	{
		//set the offset if grid is not in the Vector3.zero
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

		return grid[x, y];
	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();

		for(int x = -1; x<=1;x++)
			for(int y = -1; y<=1;y++)
			{
				if (x == 0 && y == 0)
					continue;

				int checkX = node.GridX + x;
				int checkY = node.GridY + y;

				if(checkX >=0 && checkX<gridSizeX && checkY>=0 && checkY<gridSizeY)
					neighbours.Add(grid[checkX, checkY]);
			}

		return neighbours;
	}
}
