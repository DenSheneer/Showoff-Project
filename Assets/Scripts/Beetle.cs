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
    TutorialIcon tutorialIcon;

    private void Start()
    {
        maxScaleFactor = 1.5f;
        scaleSpeed = 0.75f;
        NewRandomDestination(minRoamDist, maxRoamDist);
    }
    protected override void OnInRangeEnter()
    {
        base.OnInRangeEnter();

        tutorialIcon = new TutorialIcon(transform, TutorialType.TAB_BEETLE);
    }
    protected override void OnInRangeStay()
    {
        base.OnInRangeStay();


        tutorialIcon.UpdateIcon();
    }
    protected override void OnExitRange()
    {
        base.OnExitRange();

        tutorialIcon.Destroy();
        tutorialIcon = null;
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
    public void NewRandomDestination(float minDist, float maxDist)
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
    bool moveTowardsDestination(Vector3 target)
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        Vector3 delta = target - transform.position;
        if (Vector3.SqrMagnitude(delta) < 1.0f)
            return true;
        else
            return false;
    }
    public void RotateTowardsTarget(Vector3 delta)
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
