using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class CollectableByTongue : TapAble
{
    [SerializeField]
    protected int value = 1;
    protected NavMeshAgent agent;

    [SerializeField]
    private float minRotateSpeed = 50;
    [SerializeField]
    private float maxRotateSpeed = 50;

    protected float minScaleFactor = 1.0f, maxScaleFactor = 2.0f, scaleSpeed = 1.00f;
    protected Vector3 originalScale;
    FloatingBehaviour floatingBehaviour;

    [SerializeField]
    protected Transform GFXTransform;

    [SerializeField]
    protected float moveSpeed = 1.0f, minRoamDist = 1.1f, maxRoamDist = 2.0f;

    int obstacleLayer;

    public int Value { get => value; }
    public Vector3 Position { get => transform.position; }

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.updateRotation = false;

        originalScale = GFXTransform.localScale;

        EnterEvent += collectable_OnInRange;
        EnterStayEvent += collectable_InRangeStay;
        ExitEvent += collectable_OnExit;

        obstacleLayer = LayerMask.GetMask("Obstacles");
    }

    private void Start()
    {
        NewRandomDestination(minRoamDist, maxRoamDist);
    }

    protected void LateUpdate()
    {
        if (!agent.isStopped)
        {
            if (agent.velocity != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);

            if (Vector3.Distance(transform.position, agent.destination) < 0.1f)
            {
                NewRandomDestination(minRoamDist, maxRoamDist);
            }
        }
    }

    public void Collect(TongueController collector)
    {
        agent.radius = 0.0f;
        agent.height = 0.0f;
        transform.parent = collector.transform;
        floatingBehaviour = null;
    }


    public void SpawnAtTarget(Transform target, float minDist, float maxDist)               // Spawn Beetle in line of sight of the target.
    {
        float randomDistance = Random.Range(minDist, maxDist);
        Vector3 randomDirection = CollectableByTongue.randomDirection();
        Vector3 tempTarget = new Vector3(target.position.x, 0, target.position.z);

        RaycastHit hitRay = rayLoS(tempTarget, randomDirection, randomDistance);

        if (hitRay.collider == null)
        {
            transform.position = tempTarget + randomDirection * randomDistance;
        }
        else
        {
            transform.position = hitRay.point;
        }
    }

    public void NewRandomDestination(float minDist, float maxDist)
    {

        Vector3 newDest = transform.position + Random.insideUnitSphere * Random.Range(minDist, maxDist);
        agent.destination = newDest;
    }

    RaycastHit rayLoS(Vector3 from, Vector3 towards, float distance)                // Checks Line of sight with target.
    {
        RaycastHit hitRay;

        Physics.Raycast(from, towards, out hitRay, distance, obstacleLayer);
        Debug.DrawRay(from, towards * hitRay.distance, Color.red, 0.25f);
        return hitRay;
    }

    static Vector3 randomDirection() // Returns a random angle with unit length. (1-360 degrees)
    {
        Vector3 newDir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        newDir.Normalize();

        return newDir;
    }
    void collectable_OnExit(TapAble tapAble)
    {
        GFXTransform.localScale = originalScale;
        floatingBehaviour = null;
    }

    private void collectable_OnInRange(TapAble tapAble)
    {
        floatingBehaviour = new FloatingBehaviour(scaleSpeed, minScaleFactor, maxScaleFactor);
    }
    private void collectable_InRangeStay(TapAble tapAble)
    {
        if (floatingBehaviour != null)
            GFXTransform.localScale = originalScale * floatingBehaviour.GetScaleFactor();
    }


    public Transform GetGFXTransform()
    {
        return GFXTransform;
    }

    public static void Disable(CollectableByTongue collectable)
    {
        collectable.OutOfRange();
        collectable.gameObject.SetActive(false);
    }
}
