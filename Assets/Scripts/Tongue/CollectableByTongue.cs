using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableByTongue : TapAble
{
    [SerializeField]
    private int collectingWeight = 1;

    // InRange shrink-expand variables:
    float minScaleFactor = 1.0f, maxScaleFactor = 1.25f, scaleSpeed = 0.30f;
    Vector3 originalScale;
    FloatingBehaviour fbh;

    FirstFrameType firstFrameType = FirstFrameType.ENTERED;


    public int CollectingWeight { get => collectingWeight; }
    public Vector3 Position { get => transform.position; }

    void Start()
    {
        originalScale = transform.localScale;
        this.tag = "TongueCollectable";
    }

    public void Collect(TongueController collector)
    {
        transform.localScale = originalScale;
        transform.parent = collector.transform;
        originalScale = transform.localScale;
    }

    public override void InRange()
    {
        if (firstFrameType == FirstFrameType.ENTERED)   // When in range, do something once
        {
            firstFrameType = FirstFrameType.EXITED;

            //  In range single exec code here:
            fbh = new FloatingBehaviour(scaleSpeed, minScaleFactor, maxScaleFactor);
        }

        //  In range loop here:
        transform.localScale = originalScale * fbh.GetScaleFactor();
        return;
    }
    public override void OutOfRange()
    {
        if (firstFrameType == FirstFrameType.EXITED)  // When out of range, do something once
        {
            firstFrameType = FirstFrameType.ENTERED;

            //  Out of range single exec code here:
            gameObject.transform.localScale = originalScale;
            fbh = null;
        }
        //  Out of range loop here:

    }
}

public enum FirstFrameType { ENTERED, EXITED }
