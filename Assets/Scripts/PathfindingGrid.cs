using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
	public Transform test;
	public LayerMask impassable;
	public Transform baseCorner;
	public Transform xCorner;
	public Transform zCorner;
	public float nodeSize;
	public int area;
	private int gridXCount;
	private int gridZCount;
	private float xDiff;
	private float zDiff;
	private Vector3 boxCheckSize;
	Node[,] grid;

	void Awake()
	{
		xDiff = xCorner.position.x - baseCorner.position.x;
		zDiff = zCorner.position.z - baseCorner.position.z;
		gridXCount = Mathf.RoundToInt(xDiff / nodeSize);
		gridZCount = Mathf.RoundToInt(zDiff / nodeSize);
		boxCheckSize = new Vector3(nodeSize / 2 - .01f, nodeSize / 2 - .01f, nodeSize / 2 - .01f);
		area = gridXCount * gridZCount;
		CreateGrid();
	}
	void CreateGrid()
	{
		grid = new Node[gridXCount, gridZCount];
		Vector3 pos = new Vector3(baseCorner.position.x + nodeSize / 2, baseCorner.position.y + nodeSize / 2, baseCorner.position.z + nodeSize / 2);
		for (int x = 0; x < gridXCount; x++)
		{
			for (int z = 0; z < gridZCount; z++)
			{
				bool blocked = (Physics.CheckBox(pos, boxCheckSize, Quaternion.identity, impassable));
				grid[x, z] = new Node(pos, blocked, x ,z);
				pos.z += nodeSize;
			}
			pos.z = (baseCorner.position.z + nodeSize / 2);
			pos.x += nodeSize;
		}
	}

	public Node GetNodeFromPosition(Vector3 objectPosition)
	{
		float xOffset = (objectPosition.x - baseCorner.position.x - nodeSize / 2) / nodeSize;
		float zOffset = (objectPosition.z - baseCorner.position.z - nodeSize / 2) / nodeSize;
		int x = Mathf.RoundToInt(xOffset);
		int z = Mathf.RoundToInt(zOffset);
		return grid[x, z];
	}
	/*public List<Node> path;
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(xDiff, 1, zDiff));

		
		if (grid != null)
		{
			Node testPos = GetNodeFromPosition(test);
			foreach (Node n in grid)
			{
				Gizmos.color = (n.blocked) ? Color.red : Color.white;
				if(n == testPos)
                {
					Gizmos.color = Color.green;
                }
				if(path != null)
                {
					if(path.Contains(n))
                    {
						Gizmos.color = Color.blue;
                    }
                }
				Gizmos.DrawCube(n.pos, Vector3.one * (nodeSize - .1f));
			}
		}
	}*/
	public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
		for(int x = -1; x <= 1; x++)
        {
			for(int z = -1; z <= 1; z++)
            {
				if(!(x == 0 && z == 0))
                {
					int x1 = node.x + x;
					int z1 = node.z + z;
					if (x1 >= 0 && x1 < gridXCount && z1 >= 0 && z1 < gridZCount)
					{
						neighbors.Add(grid[x1, z1]);
					}
                }
            }
        }
		return neighbors;
    }
    private void OnDestroy()
    {

	}
}