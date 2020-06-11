﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class PickupManager : MonoBehaviour
{
    [SerializeField]
    List<TapAble> tapAbles = null;

    [SerializeField]
    PlayerManager playerManager = null;

    [SerializeField]
    DebugUI debugUI;

    int tapMask;

    void OnEnable()
    {
        tapMask = LayerMask.GetMask("TapLayer");
        playerManager.SubscribeToEatEvent(updateLevelItems);

        LeanTouch.OnFingerTap += HandleFingerTap;

        GameObject go = GameObject.FindGameObjectWithTag("UI");
        debugUI = go.GetComponent<DebugUI>();

        playerManager.updateFirefliesEvent += debugUI.UpdateUI;
    }

    private void Start()
    {
        foreach (TapAble tapAble in tapAbles)
        {
            tapAble.ExitEvent += playerManager.TapAbleOutOfReach;

            if (tapAble is Lantern)
            {
                (tapAble as Lantern).SubscribeToBeetleSpawnEvent(updateLevelItems);
                (tapAble as Lantern).onLitEvent += updateLevelItems;
            }
        }                
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

    private void Update()
    {
        debugUI.UpdateUI(playerManager.NrOfFlies);
        foreach (TapAble tapAble in tapAbles)
        {
            if (playerManager.CheckInReach(tapAble))
            {
                tapAble.InRange();
                playerManager.HandleInReachTapAble(tapAble);
            }
            else
            {
                tapAble.OutOfRange();
            }
        }
    }
}
