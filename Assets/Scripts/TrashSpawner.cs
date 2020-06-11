using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class TrashSpawner : MonoBehaviour
{
    private enum SpawnWay
    {
        FORWARDVECTOR,
        MOUSEVECTOR
    }

    [SerializeField]
    private SpawnWay spawnWay = SpawnWay.FORWARDVECTOR;

    [SerializeField]
    private PlayerManager playerManager = null;

    [SerializeField]
    private List<Trash> prefabList = new List<Trash>();

    private float timer = 2f;

    [SerializeField]
    private float spawnInterval = 3.0f;

    [SerializeField]
    private float spawnHeight = 3.0f;

    [SerializeField]
    private float randomRangeSize = 3.0f;

    int groundMask;

    void Start()
    {
        groundMask = LayerMask.GetMask("RaycastGround");
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = spawnInterval;
            SpawnTrash();
        }
    }

    private void SpawnTrash()
    {
        int randomIndex = Random.Range(0, prefabList.Count);
        Vector3 trashPos = new Vector3();
        Vector3 randomAdditive = new Vector3(Random.Range(-randomRangeSize, randomRangeSize),0,Random.Range(-randomRangeSize, randomRangeSize));

        if (playerManager.IsMoving && LeanTouch.Fingers.Count > 0)
        {
            if (spawnWay == SpawnWay.MOUSEVECTOR)
            {
                LeanFinger finger = LeanTouch.Fingers[0];

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
                {
                    if (hit.collider != null)
                    {
                        Vector3 delta = hit.point - transform.position;
                        delta.Normalize();

                        trashPos = transform.position + (delta * playerManager.GetPlayerSpeed()) + new Vector3(0, spawnHeight, 0);
                    }
                }
            } else if (spawnWay == SpawnWay.FORWARDVECTOR)
            {
                trashPos = transform.position + (-transform.forward * playerManager.GetPlayerSpeed()) + new Vector3(0, spawnHeight, 0);
            }

        }
        else
        {
            trashPos = transform.position + new Vector3(0, spawnHeight, 0);
        }

        Trash trash = Instantiate(prefabList[randomIndex], trashPos+ randomAdditive, Quaternion.identity, null);
        trash.transform.localRotation = Quaternion.Euler(Random.Range(0.0f,360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        trash.RigiB.AddTorque(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f),ForceMode.VelocityChange);
    }
}
