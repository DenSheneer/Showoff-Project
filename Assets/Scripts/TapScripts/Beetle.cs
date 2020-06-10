using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Beetle : CollectableByTongue
{
    private void Start()
    {
        tapAbleType = InputType.TAP_BEETLE;
        maxScaleFactor = 1.5f;
        scaleSpeed = 0.75f;
        NewRandomDestination(minRoamDist, maxRoamDist);
    }

    private void Update()
    {
        base.Update();
    }

    public override void Tab()
    {
        return;
    }
}
