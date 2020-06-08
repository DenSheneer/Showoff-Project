using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TongueController;

[RequireComponent(typeof(FollowRaycastNavMesh))]
[RequireComponent(typeof(BeetleSpawner))]
public class PlayerManager : MonoBehaviour
{
    FollowRaycastNavMesh movementComponent = null;

    [SerializeField]
    TongueController tongueController = null;

    [SerializeField]
    private TempUpdateScore updateScore = null;

    [SerializeField]
    uint nrOfFlies = 10, nrOfBeetles = 0, score = 0;

    bool hasDragged = false, hasEatenFly = false, hasLamped = false, hasEatenBeetle = false;

    TutorialIcon tutorialIcon;

    public Vector3 Position { get => transform.position; }
    public bool IsBusy { get => tongueController.InProgress; }
    public uint NrOfFlies { get => nrOfFlies; set => nrOfFlies = value; }
    public uint Score { get => score; }

    private void Start()
    {
        movementComponent = GetComponent<FollowRaycastNavMesh>();

        tongueController.tongueReachedTarget += HandleTargetReached;
        tongueController.targetEaten += HandleTargetEaten;
        tutorialIcon = new TutorialIcon(transform, TabAbleType.SWIPE_TO_MOVE);
    }
    private void Update()
    {
        TutorialPopUps();
    }

    public bool CheckInReach(TapAble tapbAble)
    {
        return tongueController.CheckReach(tapbAble);
    }

    public void HandleInReachTapAble(TapAble tapAble)
    {
        if (tutorialIcon == null)
        {
            if (!checkTutorialCompletion(tapAble.TapAbleType))
                tutorialIcon = new TutorialIcon(transform, tapAble.TapAbleType);
        }
    }

    public void TapAbleOutOfReach(TapAble tapAble)
    {
        if (tutorialIcon != null)
        {
            if (tutorialIcon.Type == tapAble.TapAbleType)
            {
                tutorialIcon.DestroySelf();
                tutorialIcon = null;
            }
        }
        return;
    }
    public void HandleTapAble(TapAble tapAble)
    {
        if (tapAble is DragAble)
        {
            if (tongueController.InProgress)
            {
                hasDragged = true;
                tongueController.DetacheDragAble(tapAble as DragAble);
                movementComponent.reverseDirection = false;
                return;
            }
        }

        if (tapAble.IsInReach)
        {
            if (tapAble is CollectableByTongue)
            {
                handleCollectable(tapAble as CollectableByTongue);
            }
            else if (tapAble is Lamp)
            {
                Lamp lamp = tapAble as Lamp;
                if (!lamp.IsLit && nrOfFlies > 0)
                {
                    hasLamped = true;
                    lamp.LightUp();
                    nrOfFlies--;
                }
            }
            else if (tapAble is DragAble)
            {
                if (!tongueController.InProgress)
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

    public void takeDamage(uint damage)
    {
        score -= damage;
        if (score < 0)
            score = 0;

        if (updateScore != null)
            updateScore.UpdateScore(score.ToString());
    }

    private void HandleTargetEaten(TapAble collectable)
    {
        // Do stuff when a fly is eaten
        if (collectable is Fly)
        {
            hasEatenFly = true;
            nrOfFlies += ((collectable as Fly).Value);
            score += (collectable as Fly).Value;
        }
        // Do stuff when a Beetle is eaten
        if (collectable is Beetle)
        {
            hasEatenBeetle = true;
        }
        if (updateScore != null)
            updateScore.UpdateScore(score.ToString());
    }

    private void HandleTargetReached(TapAble collectable)
    {
        if (collectable is CollectableByTongue)
            (collectable as CollectableByTongue).Collect(tongueController);

        if (collectable is DragAble)
        {

        }
    }

    void TutorialPopUps()
    {
        if (tutorialIcon != null)
        {
            if (checkTutorialCompletion(tutorialIcon.Type))
            {
                tutorialIcon.DestroySelf();
                tutorialIcon = null;
            }
            else
            {
                tutorialIcon.UpdateIcon();
            }
        }
    }

    private bool checkTutorialCompletion(TabAbleType tutorialType)
    {
        switch (tutorialType)
        {
            case TabAbleType.SWIPE_TO_MOVE:
                if (movementComponent.DistanceWalked > 1.0f)
                    return true;
                break;

            case TabAbleType.TAB_FIREFLY:
                if (hasEatenFly)
                    return true;
                break;

            case TabAbleType.TAB_BEETLE:
                if (hasEatenBeetle)
                    return true;
                break;

            case TabAbleType.TAB_DRAGABLE:
                if (hasDragged)
                    return true;
                break;

            case TabAbleType.TAB_LANTERN:
                if (hasLamped || !hasEatenFly)
                    return true;
                break;
        }
        return false;
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
