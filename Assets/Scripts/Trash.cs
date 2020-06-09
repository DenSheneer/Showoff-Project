using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Trash : MonoBehaviour
{
    [SerializeField]
    private float despawnTime = 2f;
    public float speed;

    uint damage = 1;

    private Rigidbody rb = null;

    private bool hitGround = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime < 0)
            Despawn();

    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * 0.25f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && hitGround == false)
        {
            PlayerManager player = collision.collider.GetComponent<PlayerManager>();

            if (player != null)
            {
                player.takeDamage(damage);
                Despawn();
            }
        }
        
        if (collision.collider.tag == "Ground")
        {
            hitGround = true;
        }
    }
    void Despawn()
    {
        Destroy(gameObject);
    }
}
