using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : CollectableByTongue
{

    public override void Tab()
    {
        throw new System.NotImplementedException();
    }

    public void SpawnAtTarget(Transform target, float minDist, float maxDist)
    {
        float randomDistance = Random.Range(minDist, maxDist);

        Vector3 newDir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        newDir.Normalize();

        RaycastHit hitRay;
        int obstacleLayer = LayerMask.GetMask("Obstacles");
        Physics.Raycast(target.position, newDir, out hitRay, randomDistance, obstacleLayer);

        if (hitRay.collider == null)
            transform.position = target.position + randomDistance * newDir;
        else
            transform.position = hitRay.point;
    }
}
