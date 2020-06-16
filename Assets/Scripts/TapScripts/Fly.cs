using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : CollectableByTongue
{
    FloatingBehaviour floater = new FloatingBehaviour(0.75f, 1, 1.25f);
    float originalY;

    private void Start()
    {
        base.Start();

        originalY = GFXTransform.position.y;

        //  Base class values.
        tapAbleType = InputType.TAP_FIREFLY;
        moveSpeed = 1.0f;
        minRoamDist = 1.1f;
        maxRoamDist = 2.0f;
        minScaleFactor = 1.0f;
        maxScaleFactor = 1.4f;
        scaleSpeed = 0.5f;
    }

    private void Update()
    {
        base.Update();
        flyFloat();
    }

    void flyFloat()
    {
        Vector3 position = GFXTransform.position;
        GFXTransform.position = new Vector3(position.x, originalY * floater.GetScaleFactor(), position.z);
    }

    public override void Tab()
    {
        return;
    }
}
