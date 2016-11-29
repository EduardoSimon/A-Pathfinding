using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    #region FIELDS
    Grid grid;

    public Transform source;
    public Transform target;
    #endregion

    #region MONOBEHAVIOURS
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(source.position, target.position);
    }
    #endregion

    #region METHODS

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node current = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < current.fCost || openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost)
                    current = openSet[i];
            }

            openSet.Remove(current);
            closedSet.Add(current);

            //if we found the target Node exit from the loop
            if (current == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(current))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int costToNeighbour = current.gCost + GetDistance(current, neighbour);
                if(costToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = costToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }

        }
    }

    void RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;

        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        grid.path = path;
    }

    int GetDistance(Node A, Node B)
    {
        int distX = Mathf.Abs(A.gridX - B.gridX);
        int distY = Mathf.Abs(A.gridY - B.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        else
            return 14 * distX + 10 * (distY - distX);

    }
    #endregion
}
