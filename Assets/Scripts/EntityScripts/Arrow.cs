using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : Entity
{
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material purpleMaterial;

    [SerializeField] private GameObject surface;
    [SerializeField] private MeshRenderer meshRenderer;
    
    [SerializeField] private Cell associatedCell;

    public override void InitializeEntity(Cell cell, Node parent)
    {
        associatedCell = cell;
        var color = cell.GetCellColor();
        var rotation = this.gameObject.transform.rotation;

        surface.transform.Rotate(Vector3.up, CalculateRotateAngleForDirection(cell.GetPointDirection()));
        
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
                meshRenderer.material = greenMaterial;
                break;
            case Cell.CellColor.Yellow:
                meshRenderer.material = yellowMaterial;
                break;
            case Cell.CellColor.Red:
                meshRenderer.material = redMaterial;
                break;
            case Cell.CellColor.Purple:
                meshRenderer.material = purpleMaterial;
                break;
            
        }
        this.gameObject.SetActive(true);
    }
    
    private float CalculateRotateAngleForDirection(PointDirection direction)
    {
        switch (direction)
        {
            case PointDirection.Down:
                return 180f;
            case PointDirection.Left:
                return 270f;
            case PointDirection.Up:
                return 0f;
            case PointDirection.Right:
                return 90f;
            default:
                return 0f;
        }
    }
}
