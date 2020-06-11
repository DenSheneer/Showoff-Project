using System;
using UnityEngine;

public class FloatingBehaviour
{
    private float min, max, speed;
    private float scaleFactor;

    public FloatingBehaviour(float pSpeed = 1, float pMin = 1, float pMax = 2)
    {
        min = pMin;
        max = pMax;
        speed = pSpeed;
        scaleFactor = min;
    }

    public float GetScaleFactor()
    {
        updateResult();
        return scaleFactor;
    }

    public void Restart()
    {
        scaleFactor = min;
    }

    private void updateResult()
    {
        scaleFactor += speed * Time.deltaTime;

        if (scaleFactor <= min)
        {
            scaleFactor = min;
            speed *= -1;
        }
        if (scaleFactor >= max)
        {
            scaleFactor = max;
            speed *= -1;
        }
    }
}