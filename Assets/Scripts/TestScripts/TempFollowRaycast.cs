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
                RaycastHit obstacleHit;

                if (Physics.Linecast(transform.position, hit.point,out obstacleHit, layerMask))
                {
                    agent.SetDestination(obstacleHit.point);
                }
                else
                {
                    agent.SetDestination(hit.point);
                }
            }
        } 
        else if (Input.GetMouseButtonUp(0))
        {
            agent.speed = 0;
        }
        
    }

    private void DrawRaycastToTarget(Vector3 target)
    {

    }
}
