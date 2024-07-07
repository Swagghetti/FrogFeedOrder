using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private List<Cell> cells;
    
    [SerializeField] private Node topNeighborNode;
    [SerializeField] private Node rightNeighborNode;
    [SerializeField] private Node bottomNeighborNode;
    [SerializeField] private Node leftNeighborNode;

    private void Start()
    {
        ValidateNeighboringNodes();
    }

    private void ValidateNeighboringNodes()
    {
        if (topNeighborNode != null)
        {
            if (!topNeighborNode.IsNeighborWith(this, RelativeDirection.Down))
            {
                Debug.LogWarning("Node neighboring missing: " + this.gameObject.name);
            }
            
        }
        if (rightNeighborNode != null)
        {
            if (!rightNeighborNode.IsNeighborWith(this, RelativeDirection.Left))
            {
                Debug.LogWarning("Node neighboring missing: " + this.gameObject.name);
            }
        }
        if (bottomNeighborNode != null)
        {
            if (!bottomNeighborNode.IsNeighborWith(this, RelativeDirection.Up))
            {
                Debug.LogWarning("Node neighboring missing: " + this.gameObject.name);
            }
        }
        if (leftNeighborNode != null)
        {
            if (!leftNeighborNode.IsNeighborWith(this, RelativeDirection.Right))
            {
                Debug.LogWarning("Node neighboring missing: " + this.gameObject.name);
            }
        }
    }

    //Checks if this Node is neighboring with given Node in given relativePosition
    public bool IsNeighborWith(Node node, RelativeDirection relativePosition)
    {
        switch (relativePosition)
        {
            case RelativeDirection.Up: // upper neighbor
                return topNeighborNode == node;
            case RelativeDirection.Right: // right neighbor
                return rightNeighborNode == node;
            case RelativeDirection.Down: // bottom neighbor
                return bottomNeighborNode == node;
            case RelativeDirection.Left: // left neighbor
                return leftNeighborNode == node;
        }

        return false;
    }
    
    public enum RelativeDirection
    {
        Up,
        Right,
        Down,
        Left
    }
    
}
