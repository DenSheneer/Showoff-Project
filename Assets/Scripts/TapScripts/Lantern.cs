using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

public class Lantern : TapAble
{
    AudioSource audioSource;

    [SerializeField]
    int spawnsLeft = 2;

    [SerializeField]
    private float lightRadius = 4.0f;

    public delegate void OnLitEvent(TapAble tapAble);
    public OnLitEvent onLitEvent;

    bool isLit = false;

    [SerializeField]
    float spawnCooldown = 3.0f, minSpawnDistance = 0.01f, maxSpawnDistance = 2.5f;

    [SerializeField]
    private bool litOnSpawn = false;

    BeetleSpawner beetleSpawner;

    Renderer GFX_Renderer;
    Light GFX_Light;
    ParticleSystem GFX_ParticleEffect;

    public bool IsLit { get => isLit; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        beetleSpawner = new BeetleSpawner(transform, spawnsLeft, spawnCooldown, minSpawnDistance, maxSpawnDistance);
        tapAbleType = InputType.TAP_LANTERN;
        GFX_Renderer = GetComponentInChildren<Renderer>();
        GFX_Light = GetComponentInChildren<Light>();

        //Daan zet de particle effect aan
        GFX_ParticleEffect = GetComponentInChildren<ParticleSystem>();


        GFX_Renderer.materials[1].renderQueue = 2900;
    }

    private void Start()
    {
        if (litOnSpawn)
            LightUp();
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
            if (!litOnSpawn)
                playSound(AudioData.Lantern);

            // Daan zet emission aan
            Material mat = GFX_Renderer.materials[0];

            float emission = 10;
            Color baseColor = Color.white;

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
            // en Daan zet licht aan
            GFX_Light.intensity = 20;
            GFX_ParticleEffect.Play(true);

            mat.SetColor("_EmissionColor", finalColor);

            gameObject.layer = LayerMask.NameToLayer("Default");
            isLit = true;
            beetleSpawner.SetSpawnerActivity(true);
            onLitEvent?.Invoke(this);
        }
        return;
    }
    void playSound(string name)
    {
        if (audioSource != null)
        {
            AudioClip clip = Resources.Load<AudioClip>(AudioData.path + name);
            audioSource.PlayOneShot(clip);
        }
    }

    public bool InRadiusCheck(Vector3 target, int mask)                // Checks Line of sight with target.
    {
        RaycastHit hitRay;
        Vector3 dir = Vector3.Normalize(target - transform.position);
        Physics.Raycast(transform.position, dir, out hitRay, lightRadius, mask);

        if (hitRay.collider != null)
            return true;

        return false;
    }

    private void Update()
    {
        beetleSpawner.UpdateSpawner();
    }
}
