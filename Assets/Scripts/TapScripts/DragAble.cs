using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DragAble : TapAble
{
    private NavMeshAgent navAgent = null;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }


    public void MoveTo(Vector3 pTarget,float dragLenght = 2)
    {
        float sqrDist = Vector3.Distance(pTarget, this.transform.position);
        if (sqrDist > dragLenght)
        {
            Vector3 dir = pTarget - this.transform.position;
            dir.Normalize();
            dir *= 0.01f;
            dir *= sqrDist;
            navAgent.Move(dir);
        }
    }

    public override void Tab()
    {
        return;
    }

    protected override void OnInRangeEnter()
    {
        return;
    }

    protected override void OnInRangeStay()
    {
        return;
    }

    protected override void OnExitRange()
    {
        return;
    }

    protected override void OnOutOfRangeStay()
    {
        return;
    }
}
