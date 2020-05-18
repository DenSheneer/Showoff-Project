using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class PickupManager : MonoBehaviour
{

    [SerializeField]
    List<CollectableByTongue> collectables, collected;

    [SerializeField]
    TongueController playerTongue;

    [SerializeField]
    PlayerManager playerManager;

    int tapMask;


    void OnEnable()
    {
        tapMask = LayerMask.GetMask("TapLayer");
        LeanTouch.OnFingerTap += HandleFingerTap;
    }
    private void Start()
    {
        playerTongue.eatEvent += itemCollected;
    }

    void itemCollected(CollectableByTongue collectable)
    {
        if (collectables.Contains(collectable))
        {
            collectables.Remove(collectable);
            collected.Add(collectable);
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
                }
                    
            }
        }
    }
}
