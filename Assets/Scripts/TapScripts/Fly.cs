using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : CollectableByTongue
{
    uint value = 1;
    public uint Value { get => value; }

    private void Start()
    {
        tapAbleType = TabAbleType.TAB_FIREFLY;
    }

    public override void Tab()
    {
        Debug.Log("tapped a fly");
    }
}
