using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : TapAble
{
    [SerializeField]
    int fliesLeft = 5;

    bool isLit = false;

    [SerializeField]
    float spawnCooldown = 3.0f, minSpawnDistance = 0.01f, maxSpawnDistance = 3.0f;

    public delegate void BeetleSpawnEvent(Beetle newBeetle);
    public BeetleSpawnEvent beetleSpawnEvent;

    TutorialIcon tutorialIcon = null;

    [SerializeField]
    Beetle beetlePrefab;

    public bool IsLit { get => isLit; }

    public override void Tab()
    {
        return;
    }

    protected override void OnInRangeEnter()
    {
        tutorialIcon = new TutorialIcon(transform, TutorialType.TAB_LANTERN);
    }

    protected override void OnInRangeStay()
    {
        tutorialIcon.UpdateIcon();
    }
    protected override void OnExitRange()
    {
        tutorialIcon.Destroy();
        tutorialIcon = null;
    }

    protected override void OnOutOfRangeStay()
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

        newBeetle.transform.position += new Vector3(0, -transform.position.y + 0.05f, 0);     // Might change, this forces the new beetle's y position to be a given number.

        beetleSpawnEvent?.Invoke(newBeetle);
        fliesLeft--;
    }
}
