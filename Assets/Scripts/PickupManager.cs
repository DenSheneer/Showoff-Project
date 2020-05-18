using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public Action action;

    [SerializeField]
    List<CollectableByTongue> collectables, collected;

    [SerializeField]
    TongueController player;

    private void Start()
    {
        action = updateItemState;
    }

    private void Update()
    {
        if (player.HasEaten)
            player.EatCollectable(action);
    }
    void updateItemState()
    {
        if (player.CurrentCollect != null)
        {
            if (collectables.Contains(player.CurrentCollect))
                collectables.Remove(player.CurrentCollect);

            if (!collected.Contains(player.CurrentCollect))
                collected.Add(player.CurrentCollect);
        }
        Debug.Log("eat");
    }

}
