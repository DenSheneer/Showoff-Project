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

    private void Start()
    {
        movementComponent = GetComponent<FollowRaycastNavMesh>();
    }

    public void useTapAble(TapAble tapAble)
    {
        if (tapAble is Fly)
            eatFly(tapAble as Fly);

        else if (tapAble is HealItem)
            eatHealItem(tapAble as HealItem);

        else if (tapAble is DragAble)
            enableDragMode();

    }
    public void SubscribeToEatEvent(EatEvent eatEvent)
    {
        tongueController.eatEvent += eatEvent;
    }
    public void UnsubscribeFromEatEvent(EatEvent eatEvent)
    {
        tongueController.eatEvent -= eatEvent;
    }

    void eatFly(Fly fly)
    {
        CollectableByTongue collectable = fly.GetComponent<CollectableByTongue>();
        SubscribeToEatEvent(eatFlyEvent);
        tongueController.Collect(collectable);
    }
    void eatHealItem(HealItem healItem)
    {
        CollectableByTongue collectable = healItem.GetComponent<CollectableByTongue>();
        SubscribeToEatEvent(eatHealItemEvent);
        tongueController.Collect(collectable);
    }

    void useFly(int flies)
    {
        nrOfFlies -= flies;
    }
    void takeDamage(int damage)
    {
        health -= damage;
    }
    void enableDragMode()
    {
        Debug.Log("drag mode is now on");
    }

    void eatFlyEvent (CollectableByTongue collectable)
    {
        Fly fly = collectable.GetComponent<Fly>();
        nrOfFlies += fly.Value;
        UnsubscribeFromEatEvent(eatFlyEvent);
    }
    void eatHealItemEvent(CollectableByTongue collectable)
    {
        HealItem healItem = collectable.GetComponent<HealItem>();
        health += healItem.HealAmount;
        UnsubscribeFromEatEvent(eatHealItemEvent);
    }
}
