using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private Cell.CellColor _cellColor;
    private Node _parentNode;

    public Cell.CellColor CellColor { get; set; }
    public Node ParentNode { get; set; }

    public abstract void InitializeEntity(Cell cell, Node parent);
}
