using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autopilot : MonoBehaviour
{
    [SerializeField]
    private Transform picnicTable;

    private bool autopiloton = false;

    private FollowRaycastNavMesh FRNM;

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ayy lmao");
            FRNM = GetComponent<FollowRaycastNavMesh>();
            autopiloton = true;
            LeanTouch.OnGesture -= FRNM.handleFingerGesture;
        }
    }
    private void Update()
    {
        if (autopiloton)
        {
            automove(FRNM);
        }
        
    }
    void automove(FollowRaycastNavMesh FRNM)
    {
        FRNM.MoveTowardsTarget(picnicTable.position);
    }
}
