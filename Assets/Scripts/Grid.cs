using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Grid : MonoBehaviour {

    #region FIELDS
    public Transform player;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;
    public LayerMask unwalkableMask;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    #endregion

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        print(worldBottomLeft);

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[i, j] = new Node(walkable, worldPoint,i, j);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int checkX = node.gridX + i;
                int checkY = node.gridY + j;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentY = Mathf.Clamp01(percentY);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX); // we subtract one in order to get the index in 0 based form
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> path;

    #region GIZMOS

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3 (gridWorldSize.x, 0.1f, gridWorldSize.y));

        if (grid != null)
        {
            Node playerNode = NodeFromWorldPoint(player.position);
            foreach (Node nodo in grid)
            {
                Gizmos.color = (nodo.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(nodo))
                        Gizmos.color = Color.black;
                if (playerNode == nodo) Gizmos.color = Color.cyan;
                Gizmos.DrawCube(nodo.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
    #endregion



}
