using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Frog : Entity
{
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material purpleMaterial;

    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    [SerializeField] private Cell associatedCell;
    
    
    public override void InitializeEntity(Cell cell, Node parent)
    {
        associatedCell = cell;
        var color = cell.GetCellColor();
        var rotation = this.gameObject.transform.rotation;

        this.gameObject.transform.Rotate(Vector3.up, CalculateRotateAngleForDirection(cell.GetPointDirection()));
        
        SetColorMaterial(color);
        this.gameObject.SetActive(true);

        this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        transform.DOScale(1f, appearAnimationDuration);
    }

    protected override void SetColorMaterial(Cell.CellColor color)
    {
        switch (color)
        {
            case Cell.CellColor.Green:
                skinnedMeshRenderer.material = greenMaterial;
                break;
            case Cell.CellColor.Yellow:
                skinnedMeshRenderer.material = yellowMaterial;
                break;
            case Cell.CellColor.Red:
                skinnedMeshRenderer.material = redMaterial;
                break;
            case Cell.CellColor.Purple:
                skinnedMeshRenderer.material = purpleMaterial;
                break;
            
        }
    }
    
    private float CalculateRotateAngleForDirection(PointDirection direction)
    {
        switch (direction)
        {
            case PointDirection.Down:
                return 0f;
            case PointDirection.Left:
                return 90f;
            case PointDirection.Up:
                return 180f;
            case PointDirection.Right:
                return 270f;
            default:
                return 0f;
        }
    }
}
