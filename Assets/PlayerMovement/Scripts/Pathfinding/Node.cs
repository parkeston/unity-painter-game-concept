using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//override equals & getHashcode & == for hashset optimizing
public class Node : IHeapItem<Node>
{
	public bool Walkable { get; set; }
	public Vector3 WorldPosition { get; set; }
	public int GCost { get; set; }
	public int HCost { get; set; }
	public int GridX { get; set; }
	public int GridY { get; set; }
	public Node parent { get; set; }

	public int FCost => GCost + HCost;

	public int HeapIndex { get; set; }

	public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY )
	{
		Walkable = walkable;
		WorldPosition = worldPosition;

		GridX = gridX;
		GridY = gridY;

		GCost = 0;
		HCost = 0;
	}

	public int CompareTo(Node toCompare)
	{
		int compare = FCost.CompareTo(toCompare.FCost);

		if (compare == 0)
			compare = HCost.CompareTo(toCompare.HCost);

		//less fcost - higher priority (for heap class)
		return -compare;
	}
}
