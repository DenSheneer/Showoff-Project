using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Beetle : CollectableByTongue
{
    private void Awake()
    {
        base.Awake();

        //  base class values.
        tapAbleType = InputType.TAP_BEETLE;
        maxScaleFactor = 1.5f;
        scaleSpeed = 0.75f;
        value = 5;
    }

    public override void Tab()
    {
        return;
    }
}
