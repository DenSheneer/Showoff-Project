using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SplineMesh;

[CustomEditor(typeof(SplineToTarget))]
public class SplineToTargetEdit : Editor
{

    SplineToTarget splineTarget;
    GameObject objectTarget;

    void OnEnable()
    {
        splineTarget = (SplineToTarget)target;
        objectTarget = splineTarget.gameObject;
        splineTarget.spline = splineTarget.GetComponent<Spline>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Re-calculate Spline"))
        {
            splineTarget.CreateSpline();
        }
    }
}
