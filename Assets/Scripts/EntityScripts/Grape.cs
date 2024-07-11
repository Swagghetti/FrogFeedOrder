using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Grape : Entity
{
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material purpleMaterial;

    [SerializeField] private MeshRenderer meshRenderer;
    
    public override void InitializeEntity(Cell cell, Node parent)
    {
        var color = cell.GetCellColor();
        
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
    
    
}
