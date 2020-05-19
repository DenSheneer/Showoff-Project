using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowRaycastNavMesh : MonoBehaviour
{
    [SerializeField]
    private float minRotateSpeed = 5;
    [SerializeField]
    private float maxRotateSpeed = 50;

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
                    MoveTowardsTarget(hit.point);
                }
            }
        }
    }

    private void RotateTowardsTarget(Vector3 delta)
    {
        Quaternion rotation = Quaternion.LookRotation(delta);
        float rotateSpeed = maxRotateSpeed - delta.magnitude;
        rotateSpeed = Mathf.Clamp(rotateSpeed, minRotateSpeed, maxRotateSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    private void MoveTowardsTarget(Vector3 target)
    {
        Vector3 delta = target - transform.position;
        RotateTowardsTarget(delta);

        if (delta.magnitude > 0.5f)
        {
            agent.speed = startSpeed;
            delta.Normalize();
            agent.Move(transform.forward * Time.deltaTime * 3.0f);
        }

    }

}
