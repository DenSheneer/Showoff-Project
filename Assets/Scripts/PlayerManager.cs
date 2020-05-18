using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowRaycastNavMesh))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    FollowRaycastNavMesh movementComponent;

    [SerializeField]
    TongueController tongueController;

    private void Start()
    {
        movementComponent = GetComponent<FollowRaycastNavMesh>();
    }

    public void takeFly()
    {
        Debug.Log("took a fly");
    }
}
