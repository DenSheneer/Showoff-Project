using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.AI;

public class FollowRaycastNavMesh : MonoBehaviour
{
    private Camera mainCamera;
    private NavMeshAgent agent;
    private float startSpeed;

    private int layerMask;

    void OnEnable() { LeanTouch.OnGesture += handleFingerGesture; }
    void OnDisable() { LeanTouch.OnGesture -= handleFingerGesture; }
    void Start()
    {
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        layerMask = LayerMask.GetMask("Obstacles");
        startSpeed = agent.speed;
    }

    void Update()
    {
        agent.speed = startSpeed;
    }

    void handleFingerGesture(List<LeanFinger> fingers)
    {
        LeanFinger finger = fingers[0];
        if (finger != null)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(finger.ScreenPosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider != null)
                {
                    Vector3 delta = hit.point - transform.position;
                    if (delta.magnitude > 0.5f)
                    {
                        delta.Normalize();
                        agent.SetDestination(transform.position + delta);
                    }
                }
            }
        }
    }
}
