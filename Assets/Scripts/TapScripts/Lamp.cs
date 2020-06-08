using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Lamp : TapAble
{
    [SerializeField]
    int fliesLeft = 5;

    bool isLit = false;
    protected TabAbleType tapableType;

    [SerializeField]
    float spawnCooldown = 3.0f, minSpawnDistance = 0.01f, maxSpawnDistance = 3.0f;

    public delegate void BeetleSpawnEvent(Beetle newBeetle);
    public BeetleSpawnEvent beetleSpawnEvent;

    [SerializeField]
    Beetle beetlePrefab;

    public bool IsLit { get => isLit; }

    private void Start()
    {
        tapAbleType = TabAbleType.TAB_LANTERN;
    }

    public override void Tab()
    {
        return;
    }

    public void LightUp()
    {
        if (!isLit)
        {
            // > code that changes the graphics to a lit lamp

            isLit = true;
            StartCoroutine(flySpawnTimer(spawnCooldown));
        }
        return;
    }

    IEnumerator flySpawnTimer(float time)
    {
        yield return new WaitForSeconds(time);

        if (fliesLeft > 0)
        {
            spawnBeetle();
            StartCoroutine(flySpawnTimer(spawnCooldown));
        }

    }

    void spawnBeetle()
    {
        Beetle newBeetle = Instantiate(beetlePrefab);
        newBeetle.SpawnAtTarget(transform, minSpawnDistance, maxSpawnDistance);
        newBeetle.transform.position += new Vector3(0, -transform.position.y, 0);     // Might change, this forces the new beetle's y position to be a given number.

        beetleSpawnEvent?.Invoke(newBeetle);
        fliesLeft--;
    }
}
