using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Lamp : TapAble
{
    [SerializeField]
    int fliesLeft = 5;

    bool isLit = false;

    [SerializeField]
    float spawnCooldown = 3.0f, minSpawnDistance = 0.01f, maxSpawnDistance = 3.0f;

    BeetleSpawner beetleSpawner;

    public bool IsLit { get => isLit; }

    private void Start()
    {
        tapAbleType = TabAbleType.TAB_LANTERN;
        beetleSpawner = new BeetleSpawner(transform, 5, 3.0f);
    }

    public override void Tab()
    {
        return;
    }
    public void SubscribeToBeetleSpawnEvent(BeetleSpawner.BeetleSpawnEvent beetleSpawnEvent)
    {
        beetleSpawner.SubscribeToBeetleSpawnEvent(beetleSpawnEvent);
    }

    public void LightUp()
    {
        if (!isLit)
        {
            // > code that changes the graphics to a lit lamp

            isLit = true;
            beetleSpawner.SetSpawnerActivity(true);
        }
        return;
    }

    private void Update()
    {
        beetleSpawner.UpdateSpawner();
    }


}
