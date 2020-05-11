using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using System;

[RequireComponent(typeof(Transform))]
public class FollowRaycast : MonoBehaviour
{
    [SerializeField]
    Camera viewCamera;

    [SerializeField]
    float minSpeed = 0.1f, maxSpeed = 0.5f, accelerationFactor = 1.0f, decelerateFactor = 2.0f;

    public LineSegment testLine;

    public Vector3 currentVelocity;
    private float currentAcceleration;
    private bool isAtMaxSpeed;

    CollisionData currentCollision;

    private void Start()
    {
        currentCollision = new CollisionData();
        //application settings, putting these here for now
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        Screen.orientation = ScreenOrientation.AutoRotation;

        currentVelocity = new Vector3(0.01f, 0, 0.1f);
    }

    void OnEnable()
    {
        LeanTouch.OnGesture += HandleFingerGesture;
        haltMovement();
    }

    void OnDisable()
    {
        LeanTouch.OnGesture -= HandleFingerGesture;
    }

    void HandleFingerGesture(List<LeanFinger> fingers)
    {
        LeanFinger finger = fingers[0];
        if (finger != null)
        {
            RaycastHit hit;
            Ray ray = viewCamera.ScreenPointToRay(finger.ScreenPosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 delta = hit.point - transform.position;
                if (delta.magnitude > 5.0f)
                {
                    moveInDirection(delta);
                }
                else
                {
                    haltMovement();
                }
            }
        }
    }

    void moveInDirection(Vector3 uDirection)
    {
        uDirection.Normalize();
        if (!isAtMaxSpeed)
        {
            currentAcceleration += accelerationFactor * Time.deltaTime;
            currentVelocity = uDirection * currentAcceleration;
            if (currentAcceleration >= maxSpeed)
            {
                currentAcceleration = maxSpeed;
                isAtMaxSpeed = true;
            }
        }
        else
        {
            currentVelocity = uDirection;
        }

        RotateInDirection(uDirection);

        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity.Normalize();
            currentVelocity *= maxSpeed;
        }
    }
    void RotateInDirection(Vector3 uDirection, float rotationSpeed = 10)
    {
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, uDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void haltMovement()
    {
        isAtMaxSpeed = false;
        currentAcceleration = 0;
        slowStop();
    }

    void slowStop()
    {
        if (currentVelocity.magnitude > minSpeed)
        {
            float oldLength = currentVelocity.magnitude;
            currentVelocity.Normalize();
            currentVelocity *= oldLength - decelerateFactor * Time.deltaTime;
        }
        else
        {
            currentVelocity = Vector3.zero;
        }
    }
    private void Update()
    {
        if (LeanTouch.Fingers.Count == 0)
        {
            //haltMovement();
        }

        Vector2 posToVec2 = new Vector2(transform.position.x, transform.position.z);
        Vector2 velToVec2 = new Vector2(currentVelocity.x, currentVelocity.z) * Time.deltaTime * 100.0f;
        float radius = 5.0f;

        CollisionData collision = new CollisionData();
        collision = checkLineSegmentCollisions(posToVec2, radius, velToVec2, testLine);
        if (collision.hasCollided)
        {
            ColPosReset(collision);
        }

        transform.position += currentVelocity * Time.deltaTime * 100.0f;
        currentVelocity = new Vector3(0.01f, 0, 0.1f);

    }

    CollisionData checkLineSegmentCollisions(Vector2 point, float radius, Vector2 velocity, LineSegment line)
    {
        CollisionData result = new CollisionData();

        Vector2 lineVec = line.end - line.start;
        Vector2 oldDifferenceVec = point - line.start;


        float a = Vector2.Dot(Normal(lineVec), oldDifferenceVec) - radius;
        float b = -1 * Vector2.Dot(Normal(lineVec), velocity);

        if (a > 0 && b > 0)
        {
            float ToI;
            if (a >= 0)
            {
                ToI = a / b;
            }
            else if (a >= -radius)
            {
                ToI = 0;                    //set to 0 to prevent floating point errors
            }
            else
            {
                ToI = 2;                    //set ToI to 2 when there's no collision
            }

            if (ToI <= 1)
            {
                Vector2 PoI = point + ToI * velocity;

                float d = Vector2.Dot((PoI - line.start), lineVec.normalized);

                if (d >= 0 && d <= lineVec.magnitude)
                {
                    result.hasCollided = true;
                    result.ToI = ToI;
                    result.directionNormal = lineVec;
                    result.PoI = PoI;
                    return result;
                }
            }
        }
        return result;
    }

    void ColPosReset(CollisionData cd) 
    {

        Vector3 vec3PoI = new Vector3(cd.PoI.x, 0, cd.PoI.y);
        transform.position = vec3PoI;

        Vector2 velNormalized = new Vector2(currentVelocity.x, currentVelocity.z);
        Vector2 lineVecNormalized = (testLine.end - testLine.start);
        lineVecNormalized.Normalize();
        Vector2 newVelocity = lineVecNormalized - velNormalized;
        currentVelocity = new Vector3(newVelocity.x, 0, newVelocity.y);
        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity.Normalize();
            currentVelocity *= maxSpeed * Time.deltaTime;
        }
    }

    public struct CollisionData
    {
        public bool hasCollided;
        public float ToI;
        public Vector2 PoI;
        public Vector2 directionNormal;
    }

    public Vector2 Normal(Vector2 vector)
    {
        Vector2 result = new Vector2(-vector.y, vector.x);
        result.Normalize();

        return result;
    }
}
