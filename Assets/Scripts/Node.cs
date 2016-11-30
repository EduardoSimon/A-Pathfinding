using UnityEngine;
using System.Collections;
using System;

public class Node : IHeapItem<Node>
{
    #region FIELDS
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public Node parent;

    int heapIndex;
    #endregion 

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        gridX = _gridX;
        gridY = _gridY;
        walkable = _walkable;
        worldPosition = _worldPos;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}
