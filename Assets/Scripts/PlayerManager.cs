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

    private Dictionary<InputType, bool> tutorials = new Dictionary<InputType, bool>();
    private TutorialIcon tutorialIcon;
    private TapAble nearbyTapAble = null;
    private float hintIdleTime = 5.0f;
    private float hintTimer = 0.0f;
    private bool extraHintActive;

    public Vector3 Position { get => transform.position; }
    public bool IsBusy { get => tongueController.InProgress; }
    public uint NrOfFlies { get => nrOfFlies; set => nrOfFlies = value; }
    public uint Score { get => score; }

    private void Start()
    {
        movementComponent = GetComponent<FollowRaycastNavMesh>();

        tongueController.tongueReachedTarget += HandleTargetReached;
        tongueController.targetEaten += HandleTargetEaten;


        tutorials.Add(InputType.SWIPE_TO_MOVE, false);
        tutorials.Add(InputType.TAP_FIREFLY, false);
        tutorials.Add(InputType.TAP_BEETLE, false);
        tutorials.Add(InputType.TAP_LANTERN, false);
        tutorials.Add(InputType.TAP_DRAGABLE, false);

        tutorialIcon = new TutorialIcon(transform, InputType.SWIPE_TO_MOVE);
    }
    private void Update()
    {
        UpdateTutorialPopUps();
    }

    public bool CheckInReach(TapAble tapbAble)
    {
        return tongueController.CheckReach(tapbAble);
    }

    public void HandleInReachTapAble(TapAble tapAble)
    {
        nearbyTapAble = tapAble;        // will be overwritten
        if (tutorialIcon == null && !IsBusy)
        {
            if (!checkTutorialCompletion(tapAble.TapAbleType))                              //  Check if this interactable's tutorial is completed.
            {
                tutorialIcon = new TutorialIcon(transform, tapAble.TapAbleType);
            }
            else if (!extraHintActive && timerUntilHint(tapAble, hintIdleTime))             //  If there is no extra hint already, start a timer that counts up until
            {                                                                               //  a given idle time is reached. Then, set the interactable's tutorial 
                setTutorialCompletion(tapAble.TapAbleType, false);                          //  to 'not completed' (false)
                extraHintActive = true;
            }
        }
    }

    private bool checkTutorialCompletion(InputType tutorialType)
    {
        if (extraHintActive)
            if (movementComponent.IsMoving)
            {
                setTutorialCompletion(tutorialType, true);
                extraHintActive = false;
            }

        if (tutorialType == InputType.TAP_LANTERN)
            if (nrOfFlies < 1)
                return true;

        return tutorials[tutorialType];
    }
    private void setTutorialCompletion(InputType tutorialType, bool completion)
    {
        tutorials[tutorialType] = completion;
    }

    bool timerUntilHint(TapAble tapAble, float time)
    {
        hintTimer += Time.deltaTime;

        if (movementComponent.IsMoving)
            hintTimer = 0.0f;

        if (hintTimer >= time)
            if (tapAble == nearbyTapAble)
            {
                hintTimer = 0.0f;
                return true;
            }

        return false;
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
        if (tapAble is CollectableByTongue)
        {
            if (tapAble.IsInReach)
            {
                setTutorialCompletion(tapAble.TapAbleType, true);
                handleCollectable(tapAble as CollectableByTongue);
            }
        }
        else if (tapAble is Lamp)
        {
            if (tapAble.IsInReach)
            {
                Lamp lamp = tapAble as Lamp;
                if (!lamp.IsLit && nrOfFlies > 0)
                {
                    lamp.LightUp();
                    setTutorialCompletion(tapAble.TapAbleType, true);
                    nrOfFlies--;
                }
            }
        }
        else if (tapAble is DragAble)
        {
            if (!tongueController.InProgress)
            {
                if (tapAble.IsInReach)
                {
                    setTutorialCompletion(tapAble.TapAbleType, true);
                    tongueController.SetDragTarget(tapAble as DragAble);
                    movementComponent.reverseDirection = true;
                }
            }
            else
            {
                tongueController.DetacheDragAble(tapAble as DragAble);
                movementComponent.reverseDirection = false;
                return;
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
        if (score > damage)
            score -= damage;
        else
            score = 0;

        if (updateScore != null)
            updateScore.UpdateScore(score.ToString());
    }

    private void HandleTargetEaten(TapAble collectable)
    {
        // Do stuff when a fly is eaten
        if (collectable is Fly)
        {
            nrOfFlies += ((collectable as Fly).Value);
            score += (collectable as Fly).Value;

        }
        // Do stuff when a Beetle is eaten
        if (collectable is Beetle)
        {
        }
        if (updateScore != null)
            updateScore.UpdateScore(score.ToString());

        Destroy(collectable.gameObject);
    }

    private void HandleTargetReached(TapAble collectable)
    {
        if (collectable is CollectableByTongue)
            (collectable as CollectableByTongue).Collect(tongueController);

        if (collectable is DragAble)
        {

        }
    }
    void UpdateTutorialPopUps()
    {
        if (tutorialIcon != null)
        {
            if (movementComponent.DistanceWalked > 1.0f)
                setTutorialCompletion(InputType.SWIPE_TO_MOVE, true);

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



    public void SubscribeToEatEvent(TongueEvent eatEvent)
    {
        tongueController.targetEaten += eatEvent;
    }
    public void UnsubscribeFromEatEvent(TongueEvent eatEvent)
    {
        tongueController.targetEaten -= eatEvent;
    }
}
