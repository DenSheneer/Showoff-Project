using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DragAble : TapAble
{
    private NavMeshAgent navAgent = null;

    [SerializeField]
    float speed = 1.0f;

    private void Start()
    {
        tapAbleType = InputType.TAP_DRAGABLE;
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
            dir *= sqrDist * Time.deltaTime * 80.0f;

            navAgent.Move(dir);
        }
    }

    public override void Tab()
    {
        return;
    }
}
