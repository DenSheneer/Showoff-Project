using UnityEngine;

public class Lantern : TapAble
{
    AudioSource audioSource;

    [SerializeField]
    int spawnsLeft = 2;

    int obstacleLayer;

    [SerializeField]
    private float lightRadius = 4.0f;

    public delegate void OnLitEvent(TapAble tapAble);
    public OnLitEvent onLitEvent;

    public delegate void OnBeetleSpawn(Beetle beetle);
    public OnBeetleSpawn onBeetleSpawn;

    bool isLit = false;

    [SerializeField]
    float spawnCooldown = 3.0f, minSpawnDistance = 0.01f, maxSpawnDistance = 2.5f;

    float timer = 0.0f;

    [SerializeField]
    private bool litOnSpawn = false;

    Renderer GFX_Renderer;
    Light GFX_Light;
    ParticleSystem GFX_ParticleEffect;

    public bool IsLit { get => isLit; }
    public float LightRadius { get => lightRadius; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        tapAbleType = InputType.TAP_LANTERN;
        GFX_Renderer = GetComponentInChildren<Renderer>();
        GFX_Light = GetComponentInChildren<Light>();

        obstacleLayer = LayerMask.GetMask("Obstacles");

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

    public RaycastHit RayFromThisTo(Vector3 from, Vector3 towards, float distance, int mask)                // Checks Line of sight with target.
    {
        RaycastHit hitRay;

        Physics.Raycast(from, towards, out hitRay, distance, mask);
        return hitRay;
    }

    void SpawnBeetle()
    {
        Beetle newBeetle;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector3 randomDirection = VectorUtils.randomDirection();

        Vector3 closeToGroundPosition = VectorUtils.V2ToV3(new Vector2(transform.position.x, transform.position.z), 0.01f);

        Vector3 spawnPosition;
        RaycastHit hitray = RayFromThisTo(transform.position, randomDirection, randomDistance, obstacleLayer);
        if (hitray.collider == null)
        {
            spawnPosition = transform.position + randomDirection * randomDistance;
            //Debug.DrawLine(closeToGroundPosition, spawnPosition, Color.red, 3.0f);
        }
        else
        {
            spawnPosition = transform.position + randomDirection * (hitray.distance - 1);
            //Debug.DrawLine(closeToGroundPosition, hitray.point, Color.red, 3.0f);
        }

        newBeetle = Instantiate(Resources.Load<Beetle>("Prefabs/ScriptedBeetle"), spawnPosition, Quaternion.identity);

        onBeetleSpawn?.Invoke(newBeetle);
        spawnsLeft--;
    }

    private void Update()
    {
        if (isLit && spawnsLeft > 0)
        {
            timer += Time.deltaTime;
            if (timer >= spawnCooldown)
            {
                timer = 0.0f;
                SpawnBeetle();
            }
        }
    }
}
