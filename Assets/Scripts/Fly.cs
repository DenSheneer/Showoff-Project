using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : CollectableByTongue
{
    int value = 1;
    public int Value { get => value; }

    public override void InRange() { base.InRange(); }
    public override void OutOfRange() { base.OutOfRange(); }

    public override void Tab()
    {
        Debug.Log("tapped a fly");
    }
}
