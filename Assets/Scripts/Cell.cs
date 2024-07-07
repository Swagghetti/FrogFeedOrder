using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellColor cellColor;
    [SerializeField] private CellType cellType;
    [SerializeField] private ArrowDirection arrowDirection;

    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material purpleMaterial;

    private void Start()
    {
        InitializeCellColor();
        
    }

    private void InitializeCellColor()
    {
        switch (cellColor)
        {
            case CellColor.Blue:
                this.gameObject.GetComponent<MeshRenderer>().materials[0] = blueMaterial;
                break;
            case CellColor.Green:
                this.gameObject.GetComponent<MeshRenderer>().materials[0] = greenMaterial;
                break;
            case CellColor.Yellow:
                this.gameObject.GetComponent<MeshRenderer>().materials[0] = yellowMaterial;
                break;
            case CellColor.Red:
                this.gameObject.GetComponent<MeshRenderer>().materials[0] = redMaterial;
                break;
            case CellColor.Purple:
                this.gameObject.GetComponent<MeshRenderer>().materials[0] = purpleMaterial;
                break;
        }
    }


    public enum CellColor
    {
        Blue,
        Green,
        Yellow,
        Red,
        Purple
    }

    public enum CellType
    {
        Grape,
        Arrow,
        Frog,
    }
    
    public enum ArrowDirection
    {
        Up,
        Right,
        Down,
        Left
    }
}
