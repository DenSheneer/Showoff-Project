using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget = null;

    private Vector3 offSet = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        if (followTarget != null)
            offSet = this.transform.position - followTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = followTarget.position + offSet;
    }
}
