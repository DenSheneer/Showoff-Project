using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            StartCoroutine(Shake(0.5f, 0.3f));
        }
    }
    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 orginialPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float z = Random.Range(-0.1f, 0.1f) * magnitude;
            float y = Random.Range(-0.1f, 0.1f) * magnitude;

            transform.localPosition += new Vector3(0, y, z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = orginialPos;
    }
}
