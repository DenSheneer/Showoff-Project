using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCharController : MonoBehaviour
{
    private float speed = 2f;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.right * Time.smoothDeltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += transform.forward * Time.smoothDeltaTime * speed;

        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.right * Time.smoothDeltaTime * speed;

        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += -transform.forward * Time.smoothDeltaTime * speed;

        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0,-100*Time.smoothDeltaTime * speed, 0));
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0,100*Time.smoothDeltaTime * speed, 0));
        }
    }
}
