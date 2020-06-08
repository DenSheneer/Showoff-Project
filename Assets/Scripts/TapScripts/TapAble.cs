using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class TapAble : MonoBehaviour
{
    private FirstFrameType firstFrameType = FirstFrameType.ENTERED;
    protected TabAbleType tapAbleType;

    public delegate void OnExitEvent(TapAble tapable);
    public delegate void OnExitStayEvent(TapAble tapable);
    public delegate void OnEnterEvent(TapAble tapable);
    public delegate void OnEnterStayEvent(TapAble tapable);

    public OnExitEvent ExitEvent;
    public OnExitStayEvent ExitStayEvent;
    public OnEnterEvent EnterEvent;
    public OnEnterStayEvent EnterStayEvent;

    private bool isInReach = false;

    public bool IsInReach { get => isInReach; }
    public TabAbleType TapAbleType { get => tapAbleType; }

    public abstract void Tab();


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
            EnterEvent?.Invoke(this);
        }
        EnterStayEvent?.Invoke(this);
    }
    public void OutOfRange()
    {
        if (firstFrameType == FirstFrameType.EXITED)
        {
            isInReach = false;
            firstFrameType = FirstFrameType.ENTERED;
            ExitEvent?.Invoke(this);
        }
        ExitStayEvent?.Invoke(this);
    }
}

public enum FirstFrameType { ENTERED, EXITED }
