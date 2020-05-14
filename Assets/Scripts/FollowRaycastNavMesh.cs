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
    private int groundMask;

    void OnEnable() { LeanTouch.OnGesture += handleFingerGesture; }
    void OnDisable() { LeanTouch.OnGesture -= handleFingerGesture; }
    void Start()
    {
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        layerMask = LayerMask.GetMask("Obstacles");
        groundMask = LayerMask.GetMask("RaycastGround");
        startSpeed = agent.speed;
    }

    void Update()
    {
        if ((LeanTouch.Fingers.Count == 0))
        {
            agent.speed = 0;
        }
    }

    void handleFingerGesture(List<LeanFinger> fingers)
    {
        LeanFinger finger = fingers[0];
        if (finger != null)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(finger.ScreenPosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
            {
                if (hit.collider != null)
                {
                    RaycastHit obstacleHit;
                    agent.speed = startSpeed;

                    if (Physics.Linecast(transform.position, hit.point, out obstacleHit, layerMask))
                    {
                        agent.SetDestination(obstacleHit.point);

                        Debug.DrawLine(transform.position, obstacleHit.point, Color.red, 5f);
                    }
                    else
                    {
                        agent.SetDestination(hit.point);
                    }

                    //Vector3 delta = hit.point - transform.position;
                    //if (delta.magnitude > 0.5f)
                    //{
                    //    agent.speed = startSpeed;
                    //    delta.Normalize();
                    //    agent.SetDestination(transform.position + delta);

                    //    Debug.DrawLine(transform.position, transform.position + delta, Color.red, 5f);
                    //}
                }
            }
        }
    }
}
