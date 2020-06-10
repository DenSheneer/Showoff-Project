using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : CollectableByTongue
{
    uint value = 1;
    FloatingBehaviour fb = new FloatingBehaviour(0.75f, 1, 1.25f);
    float originalY;

    public uint Value { get => value; }

    private void Start()
    {
        originalY = GFXTransform.position.y;
        tapAbleType = InputType.TAP_FIREFLY;
        speed = 1.0f;
        minRoamDist = 1.1f;
        maxRoamDist = 2.0f;
        NewRandomDestination(minRoamDist, maxRoamDist);
    }

    private void Update()
    {
        base.Update();
        upDown();
    }

    void upDown()
    {
        Vector3 position = GFXTransform.position;
        GFXTransform.position = new Vector3(position.x, originalY * fb.GetScaleFactor(), position.z);
    }

    public override void Tab()
    {
        Debug.Log("tapped a fly");
    }
}
