using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class TapAble : MonoBehaviour
{
    private FirstFrameType firstFrameType = FirstFrameType.ENTERED;

    private bool isInReach = false;

    public bool IsInReach { get => isInReach; }

    public abstract void Tab();
    protected abstract void OnInRangeEnter();
    protected abstract void OnInRangeStay();
    protected abstract void OnExitRange();
    protected abstract void OnOutOfRangeStay();


    private void Reset()
    {
        tag = "TapAble";
    }

    private void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("TapLayer");
    }

    public void InRange()
    {
        if (firstFrameType == FirstFrameType.ENTERED)
        {
            isInReach = true;
            firstFrameType = FirstFrameType.EXITED;
            OnInRangeEnter();
        }
        OnInRangeStay();
    }
    public void OutOfRange()
    {
        if (firstFrameType == FirstFrameType.EXITED)
        {
            isInReach = false;
            firstFrameType = FirstFrameType.ENTERED;
            OnExitRange();
        }
        OnOutOfRangeStay();
    }
}

public enum FirstFrameType { ENTERED, EXITED }
