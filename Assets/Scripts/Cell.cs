using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public static readonly float CellHeight = 0.1f;
    [SerializeField] private CellColor cellColor;
    [SerializeField] private EntityType entityType;
    [SerializeField] private PointDirection direction;
    
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material purpleMaterial;

    private void Start()
    {
        InitializeCellColor();
    }

    public CellColor GetCellColor()
    {
        return cellColor;
    }

    public EntityType GetCellEntityType()
    {
        return entityType;
    }
    

    private void InitializeCellColor()
    {
        var tempMaterials = this.gameObject.GetComponent<Renderer>().materials;
        
        switch (cellColor)
        { 
            case CellColor.Green:
                tempMaterials[0] = greenMaterial;
                this.gameObject.GetComponent<Renderer>().materials = tempMaterials;
                break;
            case CellColor.Yellow:
                tempMaterials[0] = yellowMaterial;
                this.gameObject.GetComponent<Renderer>().materials = tempMaterials;
                break;
            case CellColor.Red:
                tempMaterials[0] = redMaterial;
                this.gameObject.GetComponent<Renderer>().materials = tempMaterials;
                break;
            case CellColor.Purple:
                tempMaterials[0] = purpleMaterial;
                this.gameObject.GetComponent<Renderer>().materials = tempMaterials;
                break;
        }
    }


    public enum CellColor
    {
        Green,
        Yellow,
        Red,
        Purple
    }

    public enum EntityType
    {
        Grape,
        Arrow,
        Frog,
    }
    
}

public enum PointDirection
{
    Up,
    Right,
    Down,
    Left
}
