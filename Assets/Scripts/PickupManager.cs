using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class PickupManager : MonoBehaviour
{

    [SerializeField]
    List<CollectableByTongue> collectables = null, collected = null;

    [SerializeField]
    PlayerManager playerManager = null;

    [SerializeField]
    Beetle beetlePrefab = null;

    int tapMask;

    void OnEnable()
    {
        tapMask = LayerMask.GetMask("TapLayer");
        playerManager.SubscribeToEatEvent(updateLevelItems);
        LeanTouch.OnFingerTap += HandleFingerTap;
    }

    private void Start()
    {
        StartCoroutine(flySpawnTimer(3.0f));
    }

    void HandleFingerTap(LeanFinger finger)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tapMask))
        {
            if (hit.collider != null)
            {
                TapAble tapAble = hit.collider.gameObject.GetComponent<TapAble>();
                if (tapAble != null)
                {
                    tapAble.Tab();
                    playerManager.HandleTapAble(tapAble);
                }
            }
        }
    }

    void updateLevelItems(CollectableByTongue collectable)
    {
        if (collectables.Contains(collectable))
            collectables.Remove(collectable);

        if (!collected.Contains(collectable))
            collected.Add(collectable);
    }

    void SpawnBugs()
    {
        Beetle newBeetle = Instantiate(beetlePrefab);

        newBeetle.SpawnAtTarget(playerManager.transform, 0.0f, 5.0f);
        collectables.Add(newBeetle as CollectableByTongue);

        playerManager.NrOfFlies--;
    }

    private void Update()
    {
        foreach (CollectableByTongue collectable in collectables)
        {
            if (playerManager.CheckInReach(collectable.gameObject))
                collectable.InRange();
            else
                collectable.OutOfRange();
        }
    }

    IEnumerator flySpawnTimer(float time)
    {
        yield return new WaitForSeconds(time);

        if (playerManager.NrOfFlies > 0)
            SpawnBugs();

        StartCoroutine(flySpawnTimer(3.0f));
    }
}
