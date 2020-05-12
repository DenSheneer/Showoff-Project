using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline),typeof(LinearMeshAlongSpline))]
public class TongueController : MonoBehaviour
{
    [SerializeField]
    private Transform tongueStart = null;

    private Spline spline;
    private LinearMeshAlongSpline linearMesh;

    [SerializeField]
    private float tongueSpeed = 1.0f;

    [SerializeField]
    private int StartCollectingStrenght = 1;
    private int currentCollectStrenght;
    private bool Collecting = false;

    private CollectableByTongue currenctCollect = null;
    private float eatProgress = 0;
    private bool collectableAttached = false;

    void Start()
    {
        spline = GetComponent<Spline>();
        linearMesh = GetComponent<LinearMeshAlongSpline>();
        currentCollectStrenght = StartCollectingStrenght;
    }

    void Update()
    {
        if (Collecting)
            EatLoop();
    }

    void OnTriggerEnter(Collider collider)
    {
        // We only care about TongueCollectables for now
        if (collider.tag != "TongueCollectable")
            return;

        CollectableByTongue collectable = collider.GetComponent<CollectableByTongue>();

        if (collectable == null)
            return;

        if (collectable.CollectingWeight > currentCollectStrenght)
            FailCollect(collectable);
        else
            Collect(collectable);
    }

    private void Collect(CollectableByTongue collectable)
    {
        Debug.Log("item detected");
        SetSplineToTarget(collectable.transform);
        Collecting = true;
        currenctCollect = collectable;
    }

    private void FailCollect(CollectableByTongue collectable)
    {
        Debug.Log("item to big to be eaten");
    }

    private void EatCollectable(CollectableByTongue collectable)
    {
        Destroy(currenctCollect.gameObject);
        currenctCollect = null;
        Collecting = false;
    }

    private void SetSplineToTarget(Transform pTarget)
    {
        spline.nodes[1].Position = this.transform.InverseTransformPoint(tongueStart.position);
        // All the spline nodes are relative from 1 position. So we can't just set the 2nd spline node to the position of the target.
        // Doing this is the same as putting ourselfs as the parent of the target and than getting the target's local position.
        Vector3 newSplinePos = this.transform.InverseTransformPoint(pTarget.position);
        spline.nodes[0].Direction += newSplinePos - spline.nodes[0].Position;

        spline.nodes[0].Position = newSplinePos;
    }

    private void EatLoop()
    {
        SetSplineToTarget(currenctCollect.transform);

        // Eat loop while the tongue hasn't reached yet
        if (!collectableAttached)
        {
            eatProgress += Time.smoothDeltaTime * tongueSpeed;

            if (eatProgress >= 1)
                collectableAttached = true;
        } 
        // Eat loop with the target attaced to the tongue
        else if (collectableAttached)
        {
            eatProgress -= Time.smoothDeltaTime * tongueSpeed;

            if (eatProgress <= 0)
                EatCollectable(currenctCollect);
        }

        // Because spline mesh is weird. we have to 0 means full progress and 0.99f means no progress. so we reverse eating progress
        float correctProgress = 0.99f - eatProgress;

        linearMesh.UpdateMeshInterval(correctProgress);
    }
}
