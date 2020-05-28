using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget = null;

    private Vector3 offset = new Vector3();

    void Start()
    {
        offset = this.transform.position - followTarget.position;
    }

    void Update()
    {
        this.transform.position = followTarget.transform.position + offset;
    }
}
