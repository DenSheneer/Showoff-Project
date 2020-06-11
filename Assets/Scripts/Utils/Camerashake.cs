using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerashake : MonoBehaviour
{
    FloatingBehaviour cameraShaker;
    float duration;
    Quaternion originalRotation;
    Camera cameraObject;

    private void Awake()
    {
        cameraObject = Camera.main;
        originalRotation = cameraObject.transform.rotation;
    }

    public void CameraShake(int durationMiliSeconds, float speed = 25f, float minAngle = -1f, float maxAngle = 1f)
    {
        cameraShaker = new FloatingBehaviour(speed, minAngle, maxAngle);
        duration = durationMiliSeconds * 0.001f;
    }

    private void Update()
    {
        if (cameraShaker != null)
        {
            if (duration > 0)
            {
                Vector3 newRot = originalRotation.eulerAngles + new Vector3(0, cameraShaker.GetScaleFactor(), cameraShaker.GetScaleFactor());
                cameraObject.transform.rotation = Quaternion.Euler(newRot);
                duration -= Time.deltaTime;
            }else
            {
                transform.rotation = originalRotation;
                cameraShaker = null;
            }
        }
    }

}
