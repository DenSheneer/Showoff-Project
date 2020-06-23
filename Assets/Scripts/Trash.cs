using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Trash : MonoBehaviour
{
    [SerializeField]
    private float despawnTime = 2f;

    [SerializeField]
    private float trashBouncStrenght = 1f;

    public float speed;
    public int damage = 1;
    private Rigidbody rb = null;

    Collider collider;
    AudioSource audioSource;

    public Rigidbody RigiB { get => rb; }

    private bool hitGround = false;

    private void Awake()
    {
        SwitchDifficulty(PlayerInfo.Difficulty);
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime < 0)
            Despawn();

    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * 0.50f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collider.gameObject.name == "mesh_Maze")
            Physics.IgnoreCollision(collider, collision.collider);

        if (collision.collider.tag == "Player" && hitGround == false)
        {
            PlayerManager player = collision.collider.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (name.Contains("Soda"))
                    playSound(AudioData.ImpactCan);
                else
                    playSound(AudioData.ImpactPlastic);

                player.takeDamage(damage);

                Vector3 awayDir = transform.position - player.transform.position;
                awayDir.Normalize();
                awayDir *= trashBouncStrenght;
                rb.AddForce(awayDir, ForceMode.VelocityChange);

                hitGround = true;
            }
        }

        if (collision.collider.tag == "Ground")
        {
            hitGround = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collider.gameObject.name == "mesh_Maze")
            Physics.IgnoreCollision(collider, collision.collider);
    }
    void Despawn()
    {
        Destroy(gameObject);
    }
    void playSound(string name)
    {
        if (audioSource != null)
        {
            AudioClip clip = Resources.Load<AudioClip>(AudioData.path + name);
            audioSource.PlayOneShot(clip);
        }
    }
    public void SwitchDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                damage = 1;
                break;
            case Difficulty.MEDIUM:
                damage = 2;
                break;
            case Difficulty.HARD:
                damage = 3;
                break;
        }
    }
}
