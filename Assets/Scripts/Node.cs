using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Node : MonoBehaviour
{
    private Entity entity;
    private List<Cell> cells = new List<Cell>();
    
    [SerializeField] private Node topNeighborNode;
    [SerializeField] private Node rightNeighborNode;
    [SerializeField] private Node bottomNeighborNode;
    [SerializeField] private Node leftNeighborNode;

    [SerializeField] private GameObject frogEntityPrefab;
    [SerializeField] private GameObject grapeEntityPrefab;
    [SerializeField] private GameObject arrowEntityPrefab;

    private readonly float FrogHeight = 0.05f;

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
        var entityHeight = Cell.CellHeight * cells.Count;

        if (topCell.GetCellEntityType() == Cell.EntityType.Frog)
        {
            entityHeight += FrogHeight;
        }
        else if (topCell.GetCellEntityType() == Cell.EntityType.Arrow)
        {
            entityHeight += 0.01f;
        }

        var entityPosition = new Vector3(topCell.transform.position.x,
            topCell.transform.position.y, topCell.transform.position.z - entityHeight);

        switch (topCell.GetCellEntityType())
        {
            case Cell.EntityType.Grape:
                temp = Instantiate(grapeEntityPrefab, entityPosition, Quaternion.identity, this.gameObject.transform);
                temp.SetActive(false);
                temp.transform.localPosition = new Vector3(temp.transform.localPosition.x,
                    entityHeight, temp.transform.localPosition.z);
                entity = temp.GetComponent<Grape>();
                entity.InitializeEntity(topCell, this);
                break;
            case Cell.EntityType.Frog:
                temp = Instantiate(frogEntityPrefab, entityPosition, this.transform.rotation, this.gameObject.transform);
                temp.SetActive(false);
                temp.transform.localPosition = new Vector3(temp.transform.localPosition.x,
                    entityHeight, temp.transform.localPosition.z);
                entity = temp.GetComponent<Frog>();
                entity.InitializeEntity(topCell, this);
                break;
            case Cell.EntityType.Arrow:
                temp = Instantiate(arrowEntityPrefab, entityPosition, this.transform.rotation, this.gameObject.transform);
                temp.SetActive(false);
                temp.transform.localPosition = new Vector3(temp.transform.localPosition.x,
                    entityHeight, temp.transform.localPosition.z);
                entity = temp.GetComponent<Arrow>();
                entity.InitializeEntity(topCell, this);
                break;
            default:
                break;
        }
    }
    

    public GameObject GetEntityObject()
    {
        return entity.gameObject;
    }

    public Cell GetTopCell()
    {
        if (cells.Count > 0)
        {
            return cells[0];
        }
        return null;
    }

    public void RemoveTopCell()
    {
        if (cells.Count > 0)
        {
            Cell topCell = cells[0];

            topCell.gameObject.transform.DOScale(0.1f , 0.2f).OnComplete(() =>
            {
                Destroy(topCell.gameObject);
                if (entity != null)
                {
                    Destroy(entity.gameObject);
                }

                cells.RemoveAt(0);

                if (cells.Count > 0)
                {
                    SetNodeEntity();
                }
            });
        }
    }
    

    void OnMouseDown()
    {
        Debug.Log(cells.Count);
        
        Cell topCell = GetTopCell();
        if (topCell != null)
        {
            EventManager.Instance.FrogClicked(this);
        }
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

    public bool HasUnusedHiddenFrog()
    {
        for (int i = 1; i < cells.Count; i++)
        {
            if (cells[i].GetCellEntityType() == Cell.EntityType.Frog)
            {
                return true;
            }
        }

        return false;
    }

    public Node GetNeighboringNode(PointDirection direction)
    {
        if (direction == PointDirection.Down && bottomNeighborNode != null && bottomNeighborNode.GetTopCell() != null)
        {
            return bottomNeighborNode;
        }
        else if (direction == PointDirection.Left && leftNeighborNode != null && leftNeighborNode.GetTopCell() != null)
        {
            return leftNeighborNode;
        }
        else if (direction == PointDirection.Right && rightNeighborNode != null && rightNeighborNode.GetTopCell() != null)
        {
            return rightNeighborNode;
        }
        else if (direction == PointDirection.Up && topNeighborNode != null && topNeighborNode.GetTopCell() != null)
        {
            return topNeighborNode;
        }

        return null;
    }
    
    public enum RelativeDirection
    {
        Up,
        Right,
        Down,
        Left
    }
    
}
