using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linecap : MonoBehaviour
{
    [SerializeField]
    public string startEndCap;
    public Vector3 WorldPosition
    {
        get { return transform.TransformPoint(Vector3.zero); }
    }
}
