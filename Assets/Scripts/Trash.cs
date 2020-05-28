using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Trash : MonoBehaviour
{
    [SerializeField]
    private float despawnTime = 2f;

    int damage = 1;

    private Rigidbody rb = null;

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
        rb.AddForce(0.5f * -Physics.gravity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            PlayerManager player = collision.collider.GetComponent<PlayerManager>();

            if (player != null)
            {
                player.takeDamage(damage);
                Despawn();
            }
        }
    }
    void Despawn()
    {
        Destroy(gameObject);
    }
}
