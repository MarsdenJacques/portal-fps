using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapNode<Node>
{
	public bool blocked;
	public Vector3 pos;
	public int x;
	public int z;
	public int gCost;
	public int hCost;
	public Node parent;
	public int index;

	public Node(Vector3 pos, bool blocked, int x, int z)
	{
		this.blocked = blocked;
		this.pos = pos;
		this.x = x;
		this.z = z;
	}
	public int fCost()
	{
		return gCost + hCost;
	}
	public int DistanceToNode(Node node) //change this calculation
	{
		int xOffset = Mathf.Abs(node.x - this.x);
		int zOffset = Mathf.Abs(node.z - this.z);
		if (xOffset >= zOffset)
		{
			return 10 * (xOffset - zOffset) + 14 * zOffset;
		}
		else
		{
			return 10 * (zOffset - xOffset) + 14 * xOffset;
		}
	}
	public int compareTo()
    {
		return 1;
    }
	public int Index
    {
        get
        {
			return index;
        }
		set
		{
			index = value;
		}
    }
	public int CompareTo(Node other)
    {
		int result = fCost().CompareTo(other.fCost());
		if(result == 0)
        {
			result = hCost.CompareTo(other.hCost);
        }
		return result;
    }
}
