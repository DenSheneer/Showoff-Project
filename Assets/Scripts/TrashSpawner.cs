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

    private static string prefabPath = "Prefabs/";

    [SerializeField]
    private SpawnWay spawnWay = SpawnWay.FORWARDVECTOR;

    private PlayerManager playerManager = null;
    private static Trash[][] staticPrefabArray;

    private float timer;

    [SerializeField]
    private float spawnIntervalMin = 3.0f, spawnIntervalMax = 10.0f, spawnHeight = 3.0f;

    [SerializeField]
    private float randomRangeSizeOnPlayer = 3.0f, randomRangeAroundPlayer = 5.0f, randomSpawnChance = 0.5f;

    private GameObject trashParent = null;

    int groundMask;

    private void Awake()
    {
        playerManager = transform.parent.GetComponent<PlayerManager>();
    }

    void Start()
    {
        groundMask = LayerMask.GetMask("RaycastGround");

        Trash[] bottleVariants = new Trash[] { Resources.Load<Trash>(prefabPath + "pref_WaterBottle") };
        Trash[] appleVariants = new Trash[] { Resources.Load<Trash>(prefabPath + "pref_AppleTrash") };
        Trash[] canVariants = new Trash[]
        {
            Resources.Load<Trash>(prefabPath + "pref_SodaCanTrashYellow"),
            Resources.Load<Trash>(prefabPath + "pref_SodaCanTrashBlue"),
            Resources.Load<Trash>(prefabPath + "pref_SodaCanTrashRed")
        };

        staticPrefabArray = new Trash[][]
        {
            canVariants,
            bottleVariants,
            appleVariants
        };

        timer = Random.Range(spawnIntervalMin, spawnIntervalMax);

        trashParent = new GameObject("Trash Parent");
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = Random.Range(spawnIntervalMin, spawnIntervalMax);

            float rnd = Random.Range(0.0f, 1.0f);

            if (rnd < randomSpawnChance)
            {
                SpawnTrashOnPlayer();
                randomSpawnChance -= 0.1f;
                //Debug.Log("aimed for player");
            }
            else
            {
                SpawnTreshAroundPlayer();
                randomSpawnChance += 0.1f;
            }

            //Debug.Log(randomSpawnChance);
        }
    }

    int randomIndex(int arrayLength)
    {
        int randomNr = Random.Range(0, arrayLength);
        return randomNr;
    }

    private void SpawnTreshAroundPlayer()
    {
        Vector3 trashPos = this.transform.position + new Vector3(Random.Range(-randomRangeAroundPlayer, randomRangeAroundPlayer), spawnHeight, Random.Range(-randomRangeAroundPlayer, randomRangeAroundPlayer));

        Trash[] randomTrash = staticPrefabArray[randomIndex(staticPrefabArray.Length)];
        Trash randomTrashVariant = randomTrash[randomIndex(randomTrash.Length)];

        Trash trash = Instantiate(randomTrashVariant, trashPos, Quaternion.identity, trashParent.transform);
        trash.transform.localRotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        trash.RigiB.AddTorque(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), ForceMode.VelocityChange);
    }


    private void SpawnTrashOnPlayer()
    {
        Vector3 trashPos = new Vector3();
        Vector3 randomAdditive = new Vector3(Random.Range(-randomRangeSizeOnPlayer, randomRangeSizeOnPlayer), 0, Random.Range(-randomRangeSizeOnPlayer, randomRangeSizeOnPlayer));

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
            }
            else if (spawnWay == SpawnWay.FORWARDVECTOR)
            {
                trashPos = transform.position + (transform.forward * playerManager.GetPlayerSpeed()) + new Vector3(0, spawnHeight, 0);
            }

        }
        else
        {
            trashPos = transform.position + new Vector3(0, spawnHeight, 0);
        }

        Trash[] randomTrash = staticPrefabArray[randomIndex(staticPrefabArray.Length)];
        Trash randomTrashVariant = randomTrash[randomIndex(randomTrash.Length)];

        Trash trash = Instantiate(randomTrashVariant, trashPos + randomAdditive, Quaternion.identity, trashParent.transform);
        trash.transform.localRotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        trash.RigiB.AddTorque(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), ForceMode.VelocityChange);
    }
}
