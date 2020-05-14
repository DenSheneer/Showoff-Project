using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using System;

[RequireComponent(typeof(Transform))]
public class FollowRaycast : MonoBehaviour
{
    Camera viewCamera;

    [SerializeField]
    float minSpeed = 0.1f, maxSpeed = 0.1f, accelerationFactor = 1.0f, decelerateFactor = 2.0f, rotationSpeed = 10.0f;

    [SerializeField]
    float radius = 5.0f;    //is assuming the object has a spherical collider with size (x10, y10, z10).

    private bool isAtMaxSpeed;
    private float currentAcceleration;
    private Vector3 currentVelocity;
    bool allowMovement = true;

    List<LineSegment> lines = new List<LineSegment>();

    private void Start()
    {
        viewCamera = Camera.main;

        GameObject[] gos = GameObject.FindGameObjectsWithTag("CollisionLine");
        foreach (GameObject go in gos)
        {
            LineSegment line = go.GetComponent<LineSegment>();
            if (line != null)
                lines.Add(line);
        }

        //application settings, putting these here for now.
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    void OnEnable() { LeanTouch.OnGesture += handleFingerGesture; }
    void OnDisable() { LeanTouch.OnGesture -= handleFingerGesture; }

    private void FixedUpdate()
    {
        if (LeanTouch.Fingers.Count == 0)
            normalStop();
        if (allowMovement)
        {
            checkforCollisions();
        }
        updatePosition();
    }

    void handleFingerGesture(List<LeanFinger> fingers)
    {
        LeanFinger finger = fingers[0];
        if (finger != null)
        {
            RaycastHit hit;
            Ray ray = viewCamera.ScreenPointToRay(finger.ScreenPosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Vector3 delta = hit.point - transform.position;
                    if (delta.magnitude > 5.0f)
                    {
                        rotateInDirection(delta, rotationSpeed);
                        moveInDirection(delta);
                        return;
                    }
                }
            }
            normalStop();
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

        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity.Normalize();
            currentVelocity *= maxSpeed;
        }
    }
    void rotateInDirection(Vector3 direction, float rotationSpeed)
    {
        direction.Normalize();
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void normalStop()
    {
        if (allowMovement)
        {
            isAtMaxSpeed = false;
            currentAcceleration = 0;

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
        allowMovement = true;

    }
    private void forceStop()
    {
        isAtMaxSpeed = false;
        currentAcceleration = 0;
        currentVelocity = Vector3.zero;
        allowMovement = false;
    }

    void checkforCollisions()
    {
        Vector2 posToVec2 = VectorConverter.V3ToV2(transform.position);
        Vector2 velToVec2 = VectorConverter.V3ToV2(currentVelocity) * Time.deltaTime * 100.0f;

        CollisionData collision = new CollisionData();
        collision.ToI = Mathf.Infinity;

        foreach (LineSegment line in lines)
        {
            CollisionData newCollision = checkLineSegmentCollisions(posToVec2, radius, velToVec2, line);
            if (newCollision.hasCollided && newCollision.ToI < collision.ToI)
                collision = newCollision;
        }
        if (collision.hasCollided)
            colPosReset(collision);
    }

    CollisionData checkLineSegmentCollisions(Vector2 point, float radius, Vector2 velocity, LineSegment line)
    {
        CollisionData result = new CollisionData();

        Vector2 lineVec = line.end - line.start;
        Vector2 oldDifferenceVec = point - line.start;


        float a = Vector2.Dot(Vector2.Perpendicular(lineVec).normalized, oldDifferenceVec) - radius;
        float b = -1 * Vector2.Dot(Vector2.Perpendicular(lineVec).normalized, velocity);

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
                Vector2 PoI = point + ToI * velocity + velocity.normalized;

                float d = Vector2.Dot((PoI - line.start), lineVec.normalized);

                if (d >= 0 && d <= lineVec.magnitude)
                {
                    result.hasCollided = true;
                    result.hitSurface = lineVec;
                    result.ToI = ToI;
                    result.PoI = PoI;
                    return result;
                }
            }
        }
        return result;
    }

    void colPosReset(CollisionData cd)
    {
        Vector2 v2LineNormal = Vector2.Perpendicular(cd.hitSurface).normalized;
        Vector3 v3LineNormal = VectorConverter.V2ToV3(v2LineNormal);
        Vector3 vec3PoI = VectorConverter.V2ToV3(cd.PoI);

        transform.position = vec3PoI + v3LineNormal;
        forceStop();

    }

    void updatePosition()
    {
        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity.Normalize();
            currentVelocity *= maxSpeed * Time.deltaTime;
        }

        if (allowMovement)
        transform.position += currentVelocity * Time.deltaTime * 100.0f;
    }
}
