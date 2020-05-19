using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider),typeof(Rigidbody))]
public class Trash : MonoBehaviour
{
    [SerializeField]
    private float lifeSpan = 2f;


    void Update()
    {
        lifeSpan -= Time.deltaTime;

        if (lifeSpan <= 0)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            TempPlayerManager player = collision.collider.GetComponent<TempPlayerManager>();

            if (player != null)
            {
                player.playerHit();
                Destroy(this.gameObject);
            }
        }
    }
}
