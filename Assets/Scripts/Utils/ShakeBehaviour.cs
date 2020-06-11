using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBehaviour
{
    Transform target;
    Vector3 originalPosition;
    float timeLeft;

    float intensity;

    public ShakeBehaviour(Transform target, float intensity = 0.030f, float duration = .001f)
    {
        this.target = target;
        this.intensity = intensity;

        originalPosition = target.transform.localPosition;
        timeLeft = duration;
    }

    public bool Update()
    {

        if (timeLeft > 0)
        {
            target.localPosition += UnityEngine.Random.insideUnitSphere * intensity;
            timeLeft -= Time.deltaTime;
            return false;
        }
        else
        {
            target.localPosition = originalPosition;
            return true;
        }
    }
}
