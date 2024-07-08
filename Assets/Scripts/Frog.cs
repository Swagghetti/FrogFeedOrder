using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Entity
{
    

    public override void InitializeEntity(Cell cell, Node parent)
    {
        var color = cell.GetCellColor();
        
    }
}
