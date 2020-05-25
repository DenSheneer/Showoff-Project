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
    public bool IsBusy { get => tongueController.Collecting; }

    private void Start()
    {
        movementComponent = GetComponent<FollowRaycastNavMesh>();
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
                enableDragMode();
        }
    }
    void handleCollectable(CollectableByTongue collectable)
    {
        if (collectable is Fly)
            eatFly(collectable as Fly);

        else if (collectable is HealItem)
            eatHealItem(collectable as HealItem);
    }

    void eatFly(Fly fly)
    {
        SubscribeToEatEvent(eatFlyEvent);
        tongueController.Collect(fly);
    }
    void eatHealItem(HealItem healItem)
    {
        SubscribeToEatEvent(eatHealItemEvent);
        tongueController.Collect(healItem);
    }

    void useFly(int flies)
    {
        nrOfFlies -= flies;
    }
    public void takeDamage(int damage)
    {
        health -= damage;
    }
    void enableDragMode()
    {
        Debug.Log("drag mode is now on");
    }

    void eatFlyEvent(CollectableByTongue collectable)
    {
        Fly fly = collectable as Fly;
        nrOfFlies += fly.Value;
        UnsubscribeFromEatEvent(eatFlyEvent);
    }
    void eatHealItemEvent(CollectableByTongue collectable)
    {
        HealItem healItem = collectable as HealItem;
        health += healItem.HealAmount;
        UnsubscribeFromEatEvent(eatHealItemEvent);
    }

    public void SubscribeToEatEvent(EatEvent eatEvent)
    {
        tongueController.eatEvent += eatEvent;
    }
    public void UnsubscribeFromEatEvent(EatEvent eatEvent)
    {
        tongueController.eatEvent -= eatEvent;
    }
}
