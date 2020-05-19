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
            takeFly(1);

        else if (tapAble is HealItem)
            heal(1);

        else if (tapAble is DragAble)
            enableDragMode();

    }
    public void SubscribeToEatEvent(EatEvent eatEvent)
    {
        tongueController.eatEvent += eatEvent;
    }

    void takeFly(int flies)
    {
        Debug.Log("flies +" + flies);
        nrOfFlies += flies;
    }
    void useFly(int flies)
    {
        nrOfFlies -= flies;
    }
    void takeDamage(int damage)
    {
        health -= damage;
    }
    void heal(int healPoints)
    {
        Debug.Log("healing +" + healPoints);
        health += healPoints;
    }
    void enableDragMode()
    {
        Debug.Log("drag mode is now on");
    }
}
