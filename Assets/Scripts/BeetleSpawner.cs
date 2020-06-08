using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleSpawner
{
    int fliesLeft = 5;

    [SerializeField]
    float spawnCooldown = 3.0f, minSpawnDistance = 0.01f, maxSpawnDistance = 3.0f;

    Transform target;
    bool isActive = false;
    float timer = 0.0f;


    static readonly string prefabPath = "Prefabs/ScriptedBeetle";

    public delegate void BeetleSpawnEvent(Beetle newBeetle);
    public BeetleSpawnEvent beetleSpawnEvent;

    public BeetleSpawner(Transform target, int nrOfFlies, float spawnCooldown, float minSpawnDistance = 0.01f, float maxSpawnDistance = 3.0f)
    {
        this.target = target;
        this.fliesLeft = nrOfFlies;
        this.spawnCooldown = spawnCooldown;
        this.minSpawnDistance = minSpawnDistance;
        this.maxSpawnDistance = maxSpawnDistance;
    }


    public int FliesLeft
    {
        get => fliesLeft;
        set => fliesLeft = value;
    }

    public void SetSpawnerActivity(bool activity)
    {
        timer = 0.0f;
        isActive = activity;
    }

    public void SubscribeToBeetleSpawnEvent(BeetleSpawnEvent beetleSpawnEvent)
    {
        this.beetleSpawnEvent += beetleSpawnEvent;
    }

    public void UpdateSpawner()
    {
        if (isActive)
        {
            if (fliesLeft > 0)
            {
                timer += Time.deltaTime;
                if (timer >= spawnCooldown)
                {
                    timer = 0.0f;
                    spawnBeetle();
                }
            }
        }
    }

    void spawnBeetle()
    {
        Beetle newBeetle = GameObject.Instantiate(Resources.Load<Beetle>(prefabPath));
        newBeetle.SpawnAtTarget(target, minSpawnDistance, maxSpawnDistance);
        newBeetle.transform.position += new Vector3(0, -target.position.y, 0);     // Might change, this forces the new beetle's y position to be a given number.

        beetleSpawnEvent?.Invoke(newBeetle);
        fliesLeft--;
    }
}
