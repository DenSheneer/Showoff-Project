using System;
using UnityEngine;

public class FloatingBehaviour
{
    private float min, max, speed;
    public float result;

    public FloatingBehaviour(float pMin = -1, float pMax = 1, float pSpeed = 1)
    {
        min = pMin;
        max = pMax;
        speed = pSpeed;
        result = (min + max) * 0.5f;
    }

    public void Update()
    {

        result += speed * Time.deltaTime;

        if (result <= min)
        {
            result = min;
            speed *= -1;
        }
        if (result >= max)
        {
            result = max;
            speed *= -1;
        }
    }
}