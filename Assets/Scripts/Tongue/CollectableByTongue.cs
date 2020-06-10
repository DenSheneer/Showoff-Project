using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableByTongue : TapAble
{
    [SerializeField]
    private int collectingWeight = 1;

    // InRange shrink-expand variables:
    protected float minScaleFactor = 1.0f, maxScaleFactor = 2.0f, scaleSpeed = 1.00f;
    protected Vector3 originalScale;
    FloatingBehaviour floatingBehaviour;

    [SerializeField]
    protected Transform GFXTransform;

    bool idle = true;
    [SerializeField]
    protected float speed = 1.0f, minRoamDist = 1.1f, maxRoamDist = 2.0f;

    Vector3 currentTarget;

    public int CollectingWeight { get => collectingWeight; }
    public Vector3 Position { get => transform.position; }

    void OnEnable()
    {
        originalScale = GFXTransform.localScale;

        EnterEvent += collectable_OnInRange;
        EnterStayEvent += collectable_InRangeStay;
        ExitEvent += collectable_OnExit;
    }

    protected void Update()
    {
        if (!idle)
        {
            if (moveTowardsDestination(currentTarget))
            {
                idle = true;
                float randomIdleTime = Random.Range(0, 2.5f);
                StartCoroutine(idleUntilNextMove(randomIdleTime));
            }
        }
    }

    public void Collect(TongueController collector)
    {
        transform.parent = collector.transform;
        floatingBehaviour = null;
    }


    public void SpawnAtTarget(Transform target, float minDist, float maxDist)               // Spawn Beetle in line of sight of the target.
    {
        float randomDistance = Random.Range(minDist, maxDist);
        Vector3 newDir = randomDirection();
        RaycastHit hitRay = checkLoS(target, newDir, randomDistance);

        if (hitRay.collider == null)
            transform.position = target.position + randomDistance * newDir;
        else
            transform.position = hitRay.point;
    }
    public void NewRandomDestination(float minDist, float maxDist)                          // Returns a random angle with unit length. (0-360 degrees)
    {
        idle = false;
        float randomDistance = Random.Range(minDist, maxDist);
        Vector3 newDir = randomDirection();
        RaycastHit hitray = checkLoS(transform, newDir, randomDistance);

        if (hitray.collider == null)
            currentTarget = transform.position + randomDistance * newDir;
        else
            currentTarget = hitray.point;

        RotateTowardsTarget(currentTarget - transform.position);
    }

    RaycastHit checkLoS(Transform target, Vector3 direction, float distance)                // Checks Line of sight with target.
    {
        RaycastHit hitRay;
        Vector3 startPoint = new Vector3(target.position.x, 0.1f, target.position.z);

        int obstacleLayer = LayerMask.GetMask("Obstacles");
        Physics.Raycast(startPoint, direction, out hitRay, distance, obstacleLayer);
        return hitRay;
    }

    static Vector3 randomDirection()
    {
        Vector3 newDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        newDir.Normalize();
        return newDir;
    }
    bool moveTowardsDestination(Vector3 target)                                             // Move first --> target reached? return 'true' if yes, return 'false' if no.
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        Vector3 delta = target - transform.position;
        if (Vector3.SqrMagnitude(delta) < 1.0f)
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
        yield return new WaitForSeconds(time);
        NewRandomDestination(minRoamDist, maxRoamDist);
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
