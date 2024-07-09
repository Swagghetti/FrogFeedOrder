using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private List<Cell> cells = new List<Cell>();
    
    [SerializeField] private Node topNeighborNode;
    [SerializeField] private Node rightNeighborNode;
    [SerializeField] private Node bottomNeighborNode;
    [SerializeField] private Node leftNeighborNode;

    [SerializeField] private GameObject frogEntityPrefab;
    [SerializeField] private GameObject grapeEntityPrefab;
    [SerializeField] private GameObject arrowEntityPrefab;

    private void Start()
    {
        ValidateNeighboringNodes();
        InitializeChildrenCells();
        SetNodeEntity();
    }

    private void SetNodeEntity()
    {
        var topCell = cells[0];
        var entityColor = topCell.GetCellColor();
        GameObject temp;
        var entityHeight = cells.Count * Cell.CellHeight;

        var entityPosition = new Vector3(topCell.transform.position.x,
            topCell.transform.position.y, topCell.transform.position.z - entityHeight);

        switch (topCell.GetCellEntityType())
        {
            case Cell.EntityType.Grape:
                temp = Instantiate(grapeEntityPrefab, entityPosition, Quaternion.identity, this.gameObject.transform);
                temp.SetActive(false);
                entity = temp.GetComponent<Grape>();
                entity.InitializeEntity(topCell, this);
                break;
            case Cell.EntityType.Frog:
                temp = Instantiate(frogEntityPrefab, entityPosition, this.transform.rotation, this.gameObject.transform);
                temp.SetActive(false);
                entity = temp.GetComponent<Frog>();
                entity.InitializeEntity(topCell, this);
                break;
            case Cell.EntityType.Arrow:
                //TODO
                break;
            default:
                break;
        }
        
    }
    

    void OnMouseDown()
    {
        Debug.Log(cells.Count);
    }

    private void InitializeChildrenCells()
    {
        cells.AddRange(this.transform.GetComponentsInChildren<Cell>());
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
