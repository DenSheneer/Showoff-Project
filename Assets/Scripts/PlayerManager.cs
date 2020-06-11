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

    public delegate void UpdateFireflies(uint newNrOfFlies);
    public UpdateFireflies updateFirefliesEvent;
    private Animator animator;

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
    private float hintTimer = 0.0f;     // --> Do not change
    private bool extraHintActive;

    public Vector3 Position { get => transform.position; }
    public bool IsBusy { get => tongueController.InProgress; }
    public uint NrOfFlies { get => nrOfFlies; set => nrOfFlies = value; }
    public uint Score { get => score; }
    public bool IsMoving { get => movementComponent.IsMoving; }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        movementComponent = GetComponent<FollowRaycastNavMesh>();

        tongueController.tongueReachedTarget += HandleTargetReached;
        tongueController.targetEaten += HandleTargetEaten;


        tutorials.Add(InputType.SWIPE_TO_MOVE, false);
        tutorials.Add(InputType.TAP_FIREFLY, false);
        tutorials.Add(InputType.TAP_BEETLE, false);
        tutorials.Add(InputType.TAP_LANTERN, false);
        tutorials.Add(InputType.TAP_DRAGABLE, false);

        tutorialIcon = new TutorialIcon(InputType.SWIPE_TO_MOVE);
    }
    private void Update()
    {
        UpdateTutorialPopUps();

        animator.SetBool("anim_isWalking", movementComponent.IsMoving);
    }

    public bool CheckInReach(TapAble tapbAble)
    {
        return tongueController.CheckReach(tapbAble);
    }

    public void HandleInReachTapAble(TapAble tapAble)
    {
        nearbyTapAble = tapAble;        // --> NOTE: will be overwritten
        if (tutorialIcon == null && !IsBusy)
        {
            if (!checkTutorialCompletion(tapAble.TapAbleType))                              //  Check if this interactable's tutorial is completed.
            {
                tutorialIcon = new TutorialIcon(tapAble.TapAbleType);
            }
            else if (!extraHintActive && timerUntilHint(tapAble, hintIdleTime))             //  If there is no extra hint already, start a timer that counts up until
            {                                                                               //  a given idle time is reached. Then, set the interactable's tutorial 
                setTutorialCompletion(tapAble.TapAbleType, false);                          //  to 'not completed' (false)
                extraHintActive = true;
            }
        }
    }

    public float GetPlayerSpeed()
    {
        return movementComponent.GetAgentSpeed();
    }

    private bool checkTutorialCompletion(InputType tutorialType)
    {
        if (extraHintActive)
            if (movementComponent.IsMoving)
            {
                setTutorialCompletion(tutorialType, true);
                extraHintActive = false;
            }

        if (tutorialType == InputType.TAP_LANTERN)                                          // The lantern is an exception, as it is also unavailable (==complete)
            if (nrOfFlies < 1)                                                              // when the player is unable to light up the lamp.
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
                tutorialIcon.DisableTutorial();
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
                animator.SetBool("anim_isOpen", true);
                setTutorialCompletion(tapAble.TapAbleType, true);
                handleCollectable(tapAble as CollectableByTongue);
            }
        }
        else if (tapAble is Lantern)
        {
            if (tapAble.IsInReach)
            {
                Lantern lantern = tapAble as Lantern;
                if (!lantern.IsLit && nrOfFlies > 0)
                {
                    lantern.LightUp();
                    animator.SetTrigger("anim_tr_Lantern");
                    setTutorialCompletion(tapAble.TapAbleType, true);
                    nrOfFlies--;
                    updateFirefliesEvent(nrOfFlies);
                }
            }
        }
        else if (tapAble is DragAble)
        {
            if (!tongueController.InProgress)
            {
                if (tapAble.IsInReach)
                {
                    animator.SetBool("anim_isOpen", true);
                    setTutorialCompletion(tapAble.TapAbleType, true);
                    tongueController.SetDragTarget(tapAble as DragAble);
                    movementComponent.reverseDirection = true;
                }
            }
            else
            {
                animator.SetBool("anim_isOpen", false);
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
        Camerashake cameraObject = Camera.main.GetComponent<Camerashake>();
        cameraObject.CameraShake(200);
        animator.SetTrigger("anim_tr_Hit");

        if (score > damage)
            score -= damage;
        else
            score = 0;

        if (updateScore != null)
            updateScore.UpdateScore(score.ToString());
    }

    private void HandleTargetEaten(TapAble collectable)
    {
        animator.SetBool("anim_isOpen", false);
        if (collectable is Fly)
        {
            nrOfFlies += ((collectable as Fly).Value);
            updateFirefliesEvent(nrOfFlies);
        }
        if (collectable is Beetle)
        {
            score += (collectable as Beetle).Value;
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
                tutorialIcon.DisableTutorial();
                tutorialIcon = null;
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
