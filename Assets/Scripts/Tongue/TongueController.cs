using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;
using System;

[RequireComponent(typeof(Spline),typeof(LinearMeshAlongSpline))]
public class TongueController : MonoBehaviour
{
    public delegate void EatEvent(CollectableByTongue tongue);
    public EatEvent eatEvent;

    [SerializeField]
    private Transform tongueStart = null;

    private Spline spline;
    private LinearMeshAlongSpline linearMesh;

    [SerializeField]
    private float tongueSpeed = 1.0f;

    [SerializeField]
    private int StartCollectingStrenght = 1;
    private int currentCollectStrenght;
    public int CurrectCollectStrenght
    {
        get => currentCollectStrenght;
    }

    public CollectableByTongue CurrentCollect
    {
        get => currenctCollect;
    }

    public bool Collecting
    {
        get => collecting;
    }
    CollectableByTongue currenctCollect = null;
    private float eatProgress = 0;
    private bool collectableAttached = false;
    private bool collecting = false;

    void Start()
    {
        spline = GetComponent<Spline>();
        linearMesh = GetComponent<LinearMeshAlongSpline>();
        currentCollectStrenght = StartCollectingStrenght;
    }

    void Update()
    {
        if (collecting)
            EatLoop();
    }

    public void Collect(CollectableByTongue collectable)
    {
        SetSplineToTarget(collectable.transform);
        collecting = true;
        currenctCollect = collectable;
    }

    public void FailCollect(CollectableByTongue collectable)
    {
        //Debug.Log("item to big to be eaten");
    }

    public void EatCollectable(CollectableByTongue collectable)
    {
        // call eat events
        eatEvent?.Invoke(collectable);

        currenctCollect.gameObject.SetActive(false);
        currentCollectStrenght += currenctCollect.CollectingWeight; // currentCollectStrength is 'progress'. Not renaming for merging purposes
        currenctCollect = null;
        collecting = false;
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
        // Eat loop while the tongue hasn't reached yet
        if (!collectableAttached)
        {
            eatProgress += Time.smoothDeltaTime * tongueSpeed;
            SetSplineToTarget(currenctCollect.transform);

            if (eatProgress >= 1)
            {
                collectableAttached = true;
                currenctCollect.transform.SetParent(this.transform);
            }
        } 
        // Eat loop with the target attaced to the tongue
        else if (collectableAttached)
        {
            eatProgress -= (Time.smoothDeltaTime * tongueSpeed);

            if (eatProgress <= 0)
            {
                eatProgress = 0;
                collectableAttached = false;
                EatCollectable(currenctCollect);
                return;
            }
        }

        // Because spline mesh is weird. 0 means full progress and 0.99f means no progress. so we reverse eating progress
        float correctProgress = 0.99f - eatProgress;

        linearMesh.UpdateMeshInterval(correctProgress);

        if (collectableAttached)
            currenctCollect.transform.localPosition = linearMesh.GetIntervalPosition(correctProgress+0.05f);
    }
}
