using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autopilot : MonoBehaviour
{
    [SerializeField]
    private GameObject picnicBench;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().AutoMove(picnicBench.transform.position);
        }
    }
}
