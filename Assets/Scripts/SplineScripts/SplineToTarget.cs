using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class SplineToTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;
    [HideInInspector]
    public Spline spline = null;

    void Start()
    {
        spline = GetComponent<Spline>();

        if (target != null)
            CreateSpline();
    }

    public void CreateSpline()
    {
        spline.nodes[1].Position = new Vector3();
        // All the spline nodes are relative from 1 position. So we can't just set the 2nd spline node to the position of the target.
        // Doing this is the same as putting ourselfs as the parent of the target and than getting the target's local position.
        Vector3 newSplinePos = this.transform.InverseTransformPoint(target.position);
        spline.nodes[0].Direction += newSplinePos - spline.nodes[0].Position;

        spline.nodes[0].Position = newSplinePos;
    }

    void Update()
    {
        CreateSpline();
    }
}
