using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Trash : MonoBehaviour
{
    [SerializeField]
    private float despawnTime = 2f;

    int damage = 1;

    void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime < 0)
            Despawn();
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
