using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DragAble : TapAble
{
    [NonSerialized]
    public NavMeshAgent nma;

    private void Start()
    {
        nma = GetComponent<NavMeshAgent>();
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
