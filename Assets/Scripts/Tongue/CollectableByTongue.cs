using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableByTongue : TapAble
{
    [SerializeField]
    protected int value = 1;

    protected float minScaleFactor = 1.0f, maxScaleFactor = 2.0f, scaleSpeed = 1.00f;
    protected Vector3 originalScale;
    FloatingBehaviour floatingBehaviour;

    [SerializeField]
    protected Transform GFXTransform;

    bool idle = true;
    [SerializeField]
    protected float moveSpeed = 1.0f, minRoamDist = 1.1f, maxRoamDist = 2.0f, minIdleTime = 0f, maxIdleTime = 2.0f;

    Vector3 currentTarget;
    int obstacleLayer;

    public int Value { get => value; }
    public Vector3 Position { get => transform.position; }

    protected void OnEnable()
    {
        originalScale = GFXTransform.localScale;

        EnterEvent += collectable_OnInRange;
        EnterStayEvent += collectable_InRangeStay;
        ExitEvent += collectable_OnExit;

        obstacleLayer = LayerMask.GetMask("Obstacles");
    }

    protected void Start()
    {
        StartCoroutine(idleUntilNextMove(0.5f));
    }

    protected void Update()
    {
        if (!idle)
        {
            if (moveTowardsDestination(currentTarget))
            {
                IdleAndMove();
            }
        }
    }

    public void Collect(TongueController collector)
    {
        idle = true;
        transform.parent = collector.transform;
    }
    public void CancelCollect(TongueController collector)
    {
        currentTarget = default;
        transform.parent = null;
        transform.position = collector.transform.position;
        transform.position += new Vector3(0, 1, 0);
        GFXTransform.localPosition = Vector3.zero;

        IdleAndMove();
    }

    void IdleAndMove()
    {
        float randomIdleTime = Random.Range(minIdleTime, maxIdleTime);
        StartCoroutine(idleUntilNextMove(randomIdleTime));
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
    public void NewRandomDestination(float minDist, float maxDist)                          // Returns a random angle with unit length. (1-360 degrees)
    {
        Vector3 tempPosition = new Vector3(transform.position.x, 0.005f, transform.position.z);
        float randomDistance = Random.Range(minDist, maxDist);
        Vector3 newDir = randomDirection();

        RaycastHit hitRay = rayLoS(tempPosition, newDir, randomDistance);

        if (hitRay.collider == null)
            currentTarget = tempPosition + newDir * randomDistance;
        else
            currentTarget = hitRay.point;

        currentTarget += new Vector3(0, transform.position.y, 0);

        RotateTowardsTarget(newDir);
    }

    RaycastHit rayLoS(Vector3 from, Vector3 towards, float distance)                // Checks Line of sight with target.
    {
        RaycastHit hitRay;

        Physics.Raycast(from, towards, out hitRay, distance, obstacleLayer);
        Debug.DrawRay(from, towards * hitRay.distance, Color.red, 0.25f);
        return hitRay;
    }

    static Vector3 randomDirection()
    {
        Vector3 newDir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        newDir.Normalize();

        return newDir;
    }
    bool moveTowardsDestination(Vector3 target)                                             // Move first --> target reached? return 'true' if yes, return 'false' if no.
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        Vector3 delta = target - transform.position;
        float distanceTo = delta.magnitude;

        if (distanceTo < moveSpeed)                                                              //  WARNING: MIGHT OVERSHOOT AND CAUSE IT TO CLIP TROUGH WALLS
            return true;
        else
            return false;
    }
    public void RotateTowardsTarget(Vector3 delta)                                          // Sets the transform.forward towards target
    {
        Quaternion rotation = Quaternion.LookRotation(delta);
        transform.rotation = rotation;
    }
    IEnumerator idleUntilNextMove(float time)
    {
        idle = true;
        yield return new WaitForSeconds(time);
        NewRandomDestination(minRoamDist, maxRoamDist);
        idle = false;
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
