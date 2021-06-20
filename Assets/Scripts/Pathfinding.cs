using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pathfinding : MonoBehaviour
{
    public GameObject player;
    public PathfindingGrid grid;
    public Enemy enemyObj;
    HashSet<Node> visited;
    Heap<Node> toVisit;
    PathfindingManager pathManager;
    bool needToUpdateGrid;

    private void Awake()
    {
        needToUpdateGrid = false;
        visited = new HashSet<Node>();
        toVisit = new Heap<Node>(grid.area);
        pathManager = GameManager.manager.pathManager;
        pathManager.ReceivePathfinding(this);
    }
    public void ProcessRequest(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(MakePath(startPos, endPos));
    }
    public void EndPathfinding()
    {
        StopAllCoroutines();
    }
    IEnumerator MakePath(Vector3 startPos, Vector3 endPos)
    {
        Node start = grid.GetNodeFromPosition(startPos);
        Node goal = grid.GetNodeFromPosition(endPos);
        toVisit.Push(start);
        Vector3[] path = null;
        while (needToUpdateGrid)
        {
            yield return null;
        }
        if (!start.blocked && !goal.blocked)
        {
            while (toVisit.Count > 0)
            {
                Node current = toVisit.Pop();
                visited.Add(current);
                if (current == goal)
                {
                    path = ConstructPath(start, goal);
                    break;
                }
                foreach (Node neighbor in grid.GetNeighbors(current))
                {
                    if (current.blocked || visited.Contains(neighbor))
                    {
                        continue;
                    }
                    else
                    {
                        int newDist = current.gCost + current.DistanceToNode(neighbor);
                        if (newDist < neighbor.gCost || !toVisit.Contains(neighbor))
                        {
                            neighbor.gCost = newDist;
                            neighbor.hCost = neighbor.DistanceToNode(goal);
                            neighbor.parent = current;
                            if (!toVisit.Contains(neighbor))
                            {
                                toVisit.Push(neighbor);
                            }
                            else
                            {
                                toVisit.UpdatePostion(neighbor);
                            }
                        }
                    }
                }
            }
        }
        yield return null;
        toVisit.Reset();
        visited.Clear();
        pathManager.GiveResult(path, path != null);
    }
    Vector3[] ConstructPath(Node start, Node goal)
    {
        List<Vector3> path = new List<Vector3>();
        Node current = goal;
        while(current != start)
        {
            path.Add(current.pos);
            current = current.parent;
        }
        path.Add(current.pos);
        path.Reverse();
        return path.ToArray();
    }
    private void OnDestroy()
    {

    }
}
