using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject maincameraGO;

    [SerializeField]
    private GameObject endcameraGO;

    [SerializeField]
    private Camera endcamera;

    [SerializeField]
    private Camera maincamera;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("enteredTrigger");
            maincameraGO.SetActive(false);
            endcameraGO.SetActive(true);

            other.GetComponent<FollowRaycastNavMesh>().mainCamera = endcamera;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("exitedTrigger");
            maincameraGO.SetActive(true);
            endcameraGO.SetActive(false);

            other.GetComponent<FollowRaycastNavMesh>().mainCamera = maincamera;
        }
    }
}
