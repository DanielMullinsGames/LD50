using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable2D : Interactable
{
    protected sealed override bool CollisionIs2D => true;

    public int SortOrder => sortOrder + SortOrderAdjustment;
    protected int SortOrderAdjustment { get; set; }
    [SerializeField]
    private int sortOrder = 0;
}
