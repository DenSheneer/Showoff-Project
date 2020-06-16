using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Lantern : TapAble
{
    [SerializeField]
    int fliesLeft = 5;

    public delegate void OnLitEvent(TapAble tapAble);
    public OnLitEvent onLitEvent;

    bool isLit = false;

    [SerializeField]
    float spawnCooldown = 3.0f, minSpawnDistance = 0.01f, maxSpawnDistance = 3.0f;

    BeetleSpawner beetleSpawner;

    public bool IsLit { get => isLit; }

    private void OnEnable()
    {
        tapAbleType = InputType.TAP_LANTERN;
        beetleSpawner = new BeetleSpawner(transform, fliesLeft, spawnCooldown);
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
            //Daan zet emission aan
            Renderer renderer = GetComponentInChildren<Renderer>();
            Material mat = renderer.materials[0];

            float emission = 10;
            Color baseColor = Color.white;

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
            // en Daan zet licht aan
            Light light = GetComponentInChildren<Light>();
            light.intensity = 20;
            //

            mat.SetColor("_EmissionColor", finalColor);

            isLit = true;
            beetleSpawner.SetSpawnerActivity(true);
            onLitEvent?.Invoke(this);
        }
        return;
    }

    private void Update()
    {
        beetleSpawner.UpdateSpawner();
    }


}
