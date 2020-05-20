using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableByTongue : TapAble
{
    [SerializeField]
    private int collectingWeight = 1;
    public int CollectingWeight { get => collectingWeight; }
    public Vector3 Position { get => transform.position; }

    void Start()
    {
        this.tag = "TongueCollectable";
    }
    public override void InRange()
    {
        Debug.Log(name + " is in reach of the player.");
    }
    public override void OutOfRange()
    {
        return;
    }
}
