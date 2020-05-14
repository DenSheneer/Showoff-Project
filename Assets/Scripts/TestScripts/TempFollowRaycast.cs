using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TempFollowRaycast : MonoBehaviour
{
    private NavMeshAgent agent;
    private float startSpeed;

    private int layerMask;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        layerMask = LayerMask.GetMask("Obstacles");
        startSpeed = agent.speed;
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            agent.speed = startSpeed;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {

                Vector3 delta = hit.point - transform.position;
                if (delta.magnitude > 0.5f)
                {
                    delta.Normalize();
                    agent.SetDestination(transform.position + delta);
                }

            }
        } 
        else if (Input.GetMouseButtonUp(0))
        {
            agent.speed = 0;
        }
        
    }

    void rotateInDirection(Vector3 direction, float rotationSpeed = 10.0f)
    {
        direction.Normalize();
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
