using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
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
    private bool isMoving = false;

    float distanceWalked = 0.0f;


    float timer = 0;
    float tapDelay = 1.00f;     // Default delay for registering a tap as a gesture is 1/10 of a second .

    public bool reverseDirection = false;
    public float DistanceWalked { get => distanceWalked; }
    public bool IsMoving { get => isMoving; }

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
            stop();

    }

    public void handleFingerGesture(List<LeanFinger> fingers)
    {
        if (timer < tapDelay)
            timer += Time.deltaTime * 10.0f;
        else
        {
            LeanFinger finger = fingers[0];
            if (finger != null)
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(finger.ScreenPosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
                {
                    if (hit.collider != null)
                        MoveTowardsTarget(hit.point);
                }
            }
        }
    }

    private void LerpRotateTowardsTarget(Vector3 delta)
    {
        Quaternion rotation = Quaternion.LookRotation(delta);

        if (reverseDirection)
        {
            Vector3 rot = rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            rotation = Quaternion.Euler(rot);
        }

        float rotateSpeed = maxRotateSpeed - delta.magnitude;
        rotateSpeed = Mathf.Clamp(rotateSpeed, minRotateSpeed, maxRotateSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    public void RotateTowardsTarget(Vector3 delta)
    {
        Quaternion rotation = Quaternion.LookRotation(delta);
        transform.rotation = rotation;
    }

    private void MoveTowardsTarget(Vector3 target)
    {
        isMoving = true;

        Vector3 delta = target - transform.position;
        LerpRotateTowardsTarget(delta);

        if (delta.magnitude > 0.5f)
        {
            agent.speed = startSpeed;
            delta.Normalize();

            if (reverseDirection)
                agent.Move(-transform.forward * Time.deltaTime * agent.speed);
            else if (!reverseDirection)
                agent.Move(transform.forward * Time.deltaTime * agent.speed);

            distanceWalked += (transform.forward * Time.deltaTime * agent.speed).magnitude;
        }
    }
    private void stop()
    {
        isMoving = false;
        timer = 0;
    }

}
