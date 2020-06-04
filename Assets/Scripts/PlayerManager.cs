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

    TutorialIcon tutorialIcon;

    [SerializeField]
    int nrOfFlies = 10, score = 3;

    public int Score { get => score; }
    public Vector3 Position { get => transform.position; }
    public bool IsBusy { get => tongueController.InProgress; }
    public int NrOfFlies { get => nrOfFlies; set => nrOfFlies = value; }

    [SerializeField]
    private TempUpdateScore updateScore = null;

    private void Start()
    {
        movementComponent = GetComponent<FollowRaycastNavMesh>();
        tongueController.tongueReachedTarget += HandleTargetReached;
        tongueController.targetEaten += HandleTargetEaten;
        tutorialIcon = new TutorialIcon(transform, TutorialType.SWIPE_TO_MOVE);
    }
    private void Update()
    {
        tutorialIcon.UpdateIcon();
    }

    public bool CheckInReach(GameObject gameObject)
    {
        return tongueController.CheckReach(gameObject);
    }

    public void HandleTapAble(TapAble tapAble)
    {
        if (tapAble.IsInReach)
        {
            if (tapAble is CollectableByTongue)
            {
                handleCollectable(tapAble as CollectableByTongue);
            }
            else if (tapAble is DragAble)
            {
                Debug.Log("drag");
                if (tongueController.InProgress)
                {
                    tongueController.DetacheDragAble(tapAble as DragAble);
                    movementComponent.reverseDirection = false;
                }
                else
                {
                    tongueController.SetDragTarget(tapAble as DragAble);
                    movementComponent.reverseDirection = true;
                }
            }
            else if (tapAble is Lamp)
            {
                Lamp lamp = tapAble as Lamp;
                if (!lamp.IsLit && nrOfFlies > 0)
                {
                    lamp.LightUp();
                    nrOfFlies--;
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
        score -= damage;
        if (score < 0)
            score = 0;

        if (updateScore != null)
            updateScore.UpdateScore(score.ToString());
    }

    private void HandleTargetEaten(TapAble collectable)
    {
        // Do stuff when the fly is eaten
        if (collectable is Fly)
        {
            nrOfFlies += (collectable as Fly).Value;
            score += (collectable as Fly).Value;
        }
        // Do stuff when the heal item is eaten
        if (collectable is HealItem)
        {
            score += (collectable as HealItem).HealAmount;
        }
        if (updateScore != null)
            updateScore.UpdateScore(score.ToString());
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
