using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DragAble : TapAble
{
    private NavMeshAgent navAgent = null;
    TutorialIcon tutorialIcon = null;

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
        tutorialIcon = new TutorialIcon(transform, TutorialType.TAB_DRAGABLE);
    }

    protected override void OnInRangeStay()
    {
        tutorialIcon.UpdateIcon();
    }
    protected override void OnExitRange()
    {
        tutorialIcon.Destroy();
        tutorialIcon = null;
    }

    protected override void OnOutOfRangeStay()
    {
        return;
    }
}
