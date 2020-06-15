using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Lantern : TapAble
{
    [SerializeField]
    int spawnsLeft = 5;

    public delegate void OnLitEvent(TapAble tapAble);
    public OnLitEvent onLitEvent;

    bool isLit = false;

    [SerializeField]
    float spawnCooldown = 3.0f, minSpawnDistance = 0.01f, maxSpawnDistance = 3.0f;

    BeetleSpawner beetleSpawner;

    Renderer GFX_Renderer;
    Light GFX_Light;

    public bool IsLit { get => isLit; }

    private void OnEnable()
    {
        beetleSpawner = new BeetleSpawner(transform, spawnsLeft, spawnCooldown, minSpawnDistance, maxSpawnDistance);
        tapAbleType = InputType.TAP_LANTERN;
        GFX_Renderer = GetComponentInChildren<Renderer>();
        GFX_Light = GetComponentInChildren<Light>();
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
            // Daan zet emission aan
            Material mat = GFX_Renderer.materials[0];

            float emission = 10;
            Color baseColor = Color.white;

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
            // en Daan zet licht aan
            GFX_Light.intensity = 20;

            mat.SetColor("_EmissionColor", finalColor);


            gameObject.layer = LayerMask.NameToLayer("Default");
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
