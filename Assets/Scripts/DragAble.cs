using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DragAble : TapAble
{
    [NonSerialized]
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void InRange()
    {

    }

    public override void OutOfRange()
    {

    }

    public override void Tab()
    {
        Debug.Log("drag tab");
    }
}
