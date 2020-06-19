using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject maincameraGO;

    [SerializeField]
    private GameObject endcameraGO;

    [SerializeField]
    private Camera endcamera;

    [SerializeField]
    private Camera maincamera;

    [SerializeField]
    private CinemachineSmoothPath dolly;

    [SerializeField]
    private GameObject DebugUI;

    [SerializeField]
    private GameObject PicnicBench;

    [SerializeField]
    private CinemachineVirtualCamera VCcam;

    [SerializeField]
    private CinemachineDollyCart VCdolly;

    private bool inRange;
    private float initialDistance;
    private float Distance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            maincameraGO.SetActive(false);
            endcameraGO.SetActive(true);

            other.GetComponent<FollowRaycastNavMesh>().mainCamera = endcamera;
            dolly.transform.position = player.transform.position;
            dolly.transform.position += new Vector3(-2.7f, 1.95f, 0);

            DebugUI.GetComponent<Canvas>().worldCamera = endcamera;
            //DollyMove(other.gameObject, PicnicBench, VCcam);
            inRange = true;

            initialDistance = Vector3.Distance(PicnicBench.transform.position, player.transform.position);
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
            DebugUI.GetComponent<Canvas>().worldCamera = maincamera;

            inRange = false;
        }
    }

    private void Update()
    {
        if (inRange)
        {
            Distance = Vector3.Distance(PicnicBench.transform.position, player.transform.position);
            VCdolly.m_Position = (-Distance + initialDistance)*0.6f;
        }
    }
}
