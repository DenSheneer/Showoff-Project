using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Beetle : CollectableByTongue
{
    bool idle = true;
    float minRoamDist = 1.1f;
    float maxRoamDist = 2.0f;

    Vector3 currentTarget;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        NewRandomRoamTarget(minRoamDist, maxRoamDist);
    }

    public override void Tab()
    {
        StopAllCoroutines();
    }

    public void SpawnAtTarget(Transform target, float minDist, float maxDist)
    {
        float randomDistance = Random.Range(minDist, maxDist);
        Vector3 newDir = randomDirection();
        RaycastHit hitRay = checkLoS(target, newDir, randomDistance);

        if (hitRay.collider == null)
            transform.position = target.position + randomDistance * newDir;
        else
            transform.position = hitRay.point;
    }
    public void NewRandomRoamTarget(float minDist, float maxDist)
    {
        idle = false;
        float randomDistance = Random.Range(minDist, maxDist);
        Vector3 newDir = randomDirection();
        RaycastHit hitray = checkLoS(transform, newDir, randomDistance);

        if (hitray.collider == null)
            currentTarget = transform.position + randomDistance * newDir;
        else
            currentTarget = hitray.point;

        agent.SetDestination(currentTarget);
    }

    RaycastHit checkLoS(Transform target, Vector3 direction, float distance)
    {
        RaycastHit hitRay;
        int obstacleLayer = LayerMask.GetMask("Obstacles");
        Physics.Raycast(target.position, direction, out hitRay, distance, obstacleLayer);
        return hitRay;
    }

    static Vector3 randomDirection()
    {
        Vector3 newDir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        newDir.Normalize();
        return newDir;
    }

    private void Update()
    {
        if (!idle)
        {
            Vector3 delta = currentTarget - transform.position;
            if (Vector3.SqrMagnitude(delta) < 1.0f)
            {
                idle = true;
                float randomIdleTime = Random.Range(0, 2.5f);
                StartCoroutine(hold(randomIdleTime));
            }
        }
    }
    IEnumerator hold(float time)
    {
        yield return new WaitForSeconds(time);
        NewRandomRoamTarget(minRoamDist, maxRoamDist);
    }
}
