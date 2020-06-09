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

        if (tapAble is CollectableByTongue)
        {
            if ((tapAble as CollectableByTongue).IsInRange)
                handleCollectable(tapAble as CollectableByTongue);
        }
        else if (tapAble is DragAble)
        {

            if (tongueController.CheckReach(tapAble.gameObject))
            {
                Debug.Log("drag");

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
        SubscribeToEatEvent(CollectableByTongue.Disable);
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
            (collectable as CollectableByTongue).Collect(tongueController);
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
