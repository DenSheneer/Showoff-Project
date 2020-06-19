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
    public readonly int damage = 1;
    private Rigidbody rb = null;

    Collider collider;

    public Rigidbody RigiB { get => rb; }

    private bool hitGround = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
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
}
