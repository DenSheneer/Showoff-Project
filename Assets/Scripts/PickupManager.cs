using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{

    [SerializeField]
    List<CollectableByTongue> collectables, collected;

    [SerializeField]
    TongueController player;

    private void Start()
    {
        player.eatEvent += itemCollected;
    }

    void itemCollected(CollectableByTongue collectable)
    {
        if (collectables.Contains(collectable))
        {
            collectables.Remove(collectable);
            collected.Add(collectable);
        }
    }
}
