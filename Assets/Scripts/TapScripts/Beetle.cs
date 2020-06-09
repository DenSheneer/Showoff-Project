using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Beetle : CollectableByTongue
{
    bool idle = true;

    [SerializeField]
    float speed = 1.0f, minRoamDist = 1.1f, maxRoamDist = 2.0f;

    Vector3 currentTarget;

    private void Start()
    {
        tapAbleType = InputType.TAP_BEETLE;
        maxScaleFactor = 1.5f;
        scaleSpeed = 0.75f;
        NewRandomDestination(minRoamDist, maxRoamDist);
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

    RaycastHit checkLoS(Transform target, Vector3 direction, float distance)                // Cheks Line of sight with target.
    {
        RaycastHit hitRay;
        int obstacleLayer = LayerMask.GetMask("Obstacles");
        Physics.Raycast(target.position, direction, out hitRay, distance, obstacleLayer);
        return hitRay;
    }

    static Vector3 randomDirection()
    {
        Vector3 newDir = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f, 1f));
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

    private void Update()
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
    IEnumerator idleUntilNextMove(float time)
    {
        yield return new WaitForSeconds(time);
        NewRandomDestination(minRoamDist, maxRoamDist);
    }

    public override void Tab()
    {
        return;
    }
}
