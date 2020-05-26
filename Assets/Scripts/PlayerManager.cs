using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TongueController;

[RequireComponent(typeof(FollowRaycastNavMesh))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    FollowRaycastNavMesh movementComponent = null;

    [SerializeField]
    TongueController tongueController = null;

    float maxAngle = 85.0f;

    [SerializeField]
    int nrOfFlies = 0, health = 3;

    public int Health { get => health; }
    public Vector3 Position { get => transform.position; }
    public bool IsBusy { get => tongueController.InProgress; }

    private void Start()
    {
        movementComponent = GetComponent<FollowRaycastNavMesh>();
        tongueController.tongueReachedTarget += HandleTargetReached;
        tongueController.targetEaten += HandleTargetEaten;
    }

    public bool CheckInReach(GameObject gameObject)
    {
        return tongueController.CheckReach(gameObject);
    }

    public void HandleTapAble(TapAble tapAble)
    {
        if (CheckInReach(tapAble.gameObject))
        {
            Vector3 delta = tapAble.transform.position - transform.position;
            Vector3 forward = transform.forward;

            float currentAngle = Vector3.Angle(forward, delta);

            if (currentAngle > maxAngle)
                movementComponent.RotateTowardsTarget(delta);

            if (tapAble is CollectableByTongue)
                handleCollectable(tapAble as CollectableByTongue);

            else if (tapAble is DragAble)
            {
                if (tongueController.InProgress)
                {
                    tongueController.DetacheDragAble(tapAble as DragAble);
                    movementComponent.reverseDirection = false;
                }
                else if (!tongueController.InProgress)
                {
                    tongueController.SetDragTarget(tapAble as DragAble);
                    movementComponent.reverseDirection = true;
                }
            }
        }
    }
    void handleCollectable(CollectableByTongue collectable)
    {
        tongueController.SetCollectTarget(collectable);
    }

    public void takeDamage(int damage)
    {
        health -= damage;
    }

    private void HandleTargetEaten(TapAble collectable)
    {
        // Do stuff when the fly is eaten
        if (collectable is Fly)
        {
            nrOfFlies += (collectable as Fly).Value;
        }
        // Do stuff when the heal item is eaten
        if (collectable is HealItem)
        {
            health += (collectable as HealItem).HealAmount;
        }
    }

    private void HandleTargetReached(TapAble collectable)
    {
        if (collectable is CollectableByTongue)
        {
            collectable.transform.SetParent(tongueController.transform);
        }
        if (collectable is DragAble)
        {

        }
    }

    public void SubscribeToEatEvent(TongueEvent eatEvent)
    {
        tongueController.targetEaten += eatEvent;
    }
    public void UnsubscribeFromEatEvent(TongueEvent eatEvent)
    {
        tongueController.targetEaten -= eatEvent;
    }
}
