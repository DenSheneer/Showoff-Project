using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using Lean.Touch;
using UnityEngine.SceneManagement;

public class PickupManager : MonoBehaviour
{
    [SerializeField]
    List<TapAble> tapAbles = null;

    [SerializeField]
    PlayerManager playerManager = null;

    [SerializeField]
    Beetle beetlePrefab = null;

    [SerializeField]
    float spawnCoolDown = 10.0f;

    int tapMask;

    void OnEnable()
    {
        tapMask = LayerMask.GetMask("TapLayer");
        playerManager.SubscribeToEatEvent(updateLevelItems);

        foreach (TapAble tapaBle in tapAbles)
        {
            if (tapaBle is Lamp)
            {
                (tapaBle as Lamp).beetleSpawnEvent += updateLevelItems;
                Debug.Log("lamp found");
            }
        }

        LeanTouch.OnFingerTap += HandleFingerTap;

    }

    private void Start()
    {
        StartCoroutine(flySpawnTimer(spawnCoolDown));
    }


    public void AddTapble(TapAble pTapAble)
    {
        if (pTapAble != null && (!tapAbles.Contains(pTapAble)))
        {
            tapAbles.Add(pTapAble);
        }
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

    void updateLevelItems(TapAble tapAble)
    {
        if (tapAbles.Contains(tapAble))
            tapAbles.Remove(tapAble);
        else if (!tapAbles.Contains(tapAble))
            tapAbles.Add(tapAble);
    }

    void SpawnBugs()
    {
        Beetle newBeetle = Instantiate(beetlePrefab);

        newBeetle.SpawnAtTarget(playerManager.transform, 0.0f, 5.0f);
        tapAbles.Add(newBeetle as TapAble);

        playerManager.NrOfFlies--;
    }

    private void Update()
    {
        foreach (TapAble tapAble in tapAbles)
        {
            if (playerManager.CheckInReach(tapAble.gameObject))
                tapAble.InRange();
            else
                tapAble.OutOfRange();
        }
    }

    IEnumerator flySpawnTimer(float time)
    {
        yield return new WaitForSeconds(time);

        if (playerManager.NrOfFlies > 0)
            //SpawnBugs();

        StartCoroutine(flySpawnTimer(spawnCoolDown));
    }
}
