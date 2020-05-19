using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform camTarget = null;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - camTarget.position;
    }

    void Update()
    {
        transform.position = camTarget.position + offset;
    }
}
