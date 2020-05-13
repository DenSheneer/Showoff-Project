using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linecap : MonoBehaviour
{
    public Vector3 WorldPosition
    {
        get { return transform.TransformPoint(Vector3.zero); }
    }
    private void Start()
    {
        Debug.Log(WorldPosition);
    }
}
