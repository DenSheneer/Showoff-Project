using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : TapAble
{
    public override void InRange()
    {
        throw new System.NotImplementedException();
    }

    public override void OutOfRange()
    {
        throw new System.NotImplementedException();
    }

    public override void Tab()
    {
        Debug.Log("tapped a fly");
    }
}
