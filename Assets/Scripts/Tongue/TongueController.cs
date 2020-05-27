using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;
using System;
using UnityEditor.VersionControl;

[RequireComponent(typeof(Spline), typeof(LinearMeshAlongSpline))]
public class TongueController : MonoBehaviour
{
    private enum TongueEventType
    {
        COLLECTING,
        DRAGGING
    }

    private TongueEventType tongueEventType;

    public delegate void TongueEvent(CollectableByTongue tongue);
    // Event that is called when the tongue reached its target
    public TongueEvent tongueReachedTarget;
    // Event that is called when the tongue has eaten its target
    public TongueEvent targetEaten;

    [SerializeField]
    private Transform tongueStart = null;

    private Spline spline = null;
    private LinearMeshAlongSpline linearMesh = null;

    [SerializeField]
    private float tongueSpeed = 1.0f, reachDistance = 3.0f;

    public float Reach { get => reachDistance; }

    private TapAble tongueTarget = null;

    private bool inProgress = false;
    public bool InProgress { get => inProgress;}

    private float tongueProgress = 0;
    private bool targetReached = false;

    [SerializeField]
    private float invokeTime = 0.01f;

    void Start()
    {
        spline = GetComponent<Spline>();
        linearMesh = GetComponent<LinearMeshAlongSpline>();
    }

    void Update()
    {
        if (InProgress)
            tongueLoop();
    }

    public bool SetCollectTarget(CollectableByTongue collectable)
    {
        if (inProgress)
        {
            Debug.Log("Tongue was bussy doing something else ");

            return false;
        }
        inProgress = true;
        tongueTarget = collectable;
        tongueEventType = TongueEventType.COLLECTING;
        linearMesh.UpdateMeshFillingMode(MeshBender.FillingMode.Repeat);


        return true;
    }

    public bool SetDragTarget(DragAble dragAble)
    {
        if (inProgress)
        {
            Debug.Log("Tongue was bussy doing something else ");

            return false;
        }
        inProgress = true;
        tongueTarget = dragAble;
        tongueEventType = TongueEventType.DRAGGING;

        // TO-DO: We prop want to make the tongue go slowly back but for now we don't
        tongueProgress = 0;

        float correctProgress = 0.99f - tongueProgress;
        linearMesh.UpdateMeshFillingMode(MeshBender.FillingMode.StretchToInterval);
        linearMesh.UpdateMeshInterval(correctProgress);

        return true;
    }

    public void DetacheDragAble(DragAble dragAble)
    {
        // Can't detache if we have nothing to detache
        if (!inProgress || tongueEventType == TongueEventType.COLLECTING)
            return;

        if (tongueTarget == dragAble)
        {
            tongueTarget = null;
            inProgress = false;

            // TO-DO: We prop want to make the tongue go slowly back but for now we don't
            tongueProgress = 0;
            targetReached = false;

            float correctProgress = 0.99f - tongueProgress;
            linearMesh.UpdateMeshInterval(correctProgress);
        }
        if (IsInvoking("dragLoop"))
            CancelInvoke("dragLoop");
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


    private void dragLoop()
    {
        float dist = Vector3.Distance(this.transform.position, tongueTarget.transform.position);
        Vector3 dir = this.transform.position - tongueTarget.transform.position;
        dir.Normalize();
        if (dist > 3)
        {
            (tongueTarget as DragAble).rb.AddForce(dir * 2, ForceMode.VelocityChange);
        }
        SetSplineToTarget(tongueTarget.transform);
    }


    private void tongueLoop()
    {
        if (!targetReached)
        {
            tongueProgress += Time.smoothDeltaTime * tongueSpeed;
            SetSplineToTarget(tongueTarget.transform);

            if (tongueProgress >= 1)
            {
                targetReached = true;
                tongueReachedTarget?.Invoke(tongueTarget as CollectableByTongue);
            }

        }
        else if (tongueEventType == TongueEventType.DRAGGING)
        {
            if (!IsInvoking("dragLoop"))
                Invoke("dragLoop", invokeTime);
        }
        else if (tongueEventType == TongueEventType.COLLECTING)
        {
            tongueProgress -= Time.smoothDeltaTime * tongueSpeed;

            if (tongueProgress <= 0)
            {
                targetReached = false;
                targetEaten?.Invoke(tongueTarget as CollectableByTongue);
                inProgress = false;

                return;
            }
        }

        float correctProgress = 0.99f - tongueProgress;
        linearMesh.UpdateMeshInterval(correctProgress);

        if (targetReached && tongueEventType == TongueEventType.COLLECTING)
            tongueTarget.transform.localPosition = linearMesh.GetIntervalPosition(correctProgress + 0.05f);
    }

    public bool CheckReach(GameObject target)
    {
        RaycastHit hitRay;
        int obstacleLayer = LayerMask.GetMask("Obstacles");
        Physics.Linecast(tongueStart.position, target.transform.position, out hitRay, obstacleLayer);
        float distance = Vector3.Distance(tongueStart.position, target.transform.position);

        if (hitRay.collider == null && distance < reachDistance)
            return true;
        else
            return false;
    }
}