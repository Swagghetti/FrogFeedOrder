using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellColor _cellColor;
    [SerializeField] private CellType _cellType;
    [SerializeField] private ArrowDirection _arrowDirection;
    
    
    public enum CellColor
    {
        blue,
        green,
        yellow,
        red,
        purple
    }

    public enum CellType
    {
        grape,
        arrow,
        frog,
    }
    
    public enum ArrowDirection
    {
        up,
        right,
        down,
        left
    }
}
