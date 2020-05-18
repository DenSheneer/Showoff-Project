using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TempMoveble : MonoBehaviour
{
    public Rigidbody rb = null;

    void Start()
    {
        this.tag = "Moveable";
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
}
