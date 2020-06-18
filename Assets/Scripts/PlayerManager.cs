using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static TongueController;

[RequireComponent(typeof(FollowRaycastNavMesh))]
[RequireComponent(typeof(BeetleSpawner))]
public class PlayerManager : MonoBehaviour
{
    FollowRaycastNavMesh movementComponent = null;

    public delegate void OnTapableChange(TapAble changedTapable);
    public delegate void OnFireflyChange(uint newNrOfFireflies);

    public OnTapableChange onTapableChange;
    public OnFireflyChange onfireFlyChange;

    private static Animator animator = null;
    private static ParticleSystem particles_beetle = null;
    private static ParticleSystem particles_fly = null;

    [SerializeField]
    TongueController tongueController = null;

    [SerializeField]
    private TempUpdateScore updateScore = null;

    private Camerashake cameraObject;

    [SerializeField]
    private uint nrOfFlies = 0, nrOfBeetles = 0;
    private uint score = 0;
    public readonly uint MaxNrOfFlies = 3;

    private bool isGettingHurt = false;

    private Dictionary<InputType, bool> tutorials = new Dictionary<InputType, bool>();
    private TutorialIcon tutorialIcon;
    private TapAble nearbyTapAble = null;
    private readonly float hintIdleTime = 5.0f;
    private float hintTimer = 0.0f;     // --> Do not change
    private bool extraHintActive;

    public Vector3 Position { get => transform.position; }
    public uint NrOfFlies { get => nrOfFlies; set => nrOfFlies = value; }
    public uint Score { get => score; }
    public bool IsMoving { get => movementComponent.IsMoving; }

    public bool IsBusy()
    {
        if (!isGettingHurt)
            return tongueController.InProgress;
        else
            return isGettingHurt;
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        particles_beetle = GameObject.Find("PickupEffect").GetComponent<ParticleSystem>();
        particles_fly = GameObject.Find("PickupFirefly").GetComponent<ParticleSystem>();
        movementComponent = GetComponent<FollowRaycastNavMesh>();
        cameraObject  = Camera.main.GetComponent<Camerashake>();

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
        if (tutorialIcon == null && !IsBusy())
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
        if (!IsBusy())
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
                        onfireFlyChange?.Invoke(nrOfFlies);
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
            }
        }
        else if (tapAble is DragAble)
        {
            animator.SetBool("anim_isOpen", false);
            tongueController.DetacheDragAble(tapAble as DragAble);
            movementComponent.reverseDirection = false;
            return;
        }
    }
    void handleCollectable(CollectableByTongue collectable)
    {
        tongueController.SetCollectTarget(collectable);
        SubscribeToEatEvent(CollectableByTongue.Disable);
    }

    public void takeDamage(uint damage)
    {
        StartCoroutine(takeDamageTimer(damage));
    }

    private void HandleTargetEaten(TapAble collectable)
    {
        animator.SetBool("anim_isOpen", false);

        if (collectable is Fly)
        {
            nrOfFlies += ((collectable as Fly).Value);
            onfireFlyChange?.Invoke(nrOfFlies);

            if (particles_fly != null)
                particles_fly.Play();
        }
        if (collectable is Beetle)
        {
            score += (collectable as Beetle).Value;

            if (particles_beetle != null)
                particles_beetle.Play();
        }
        if (updateScore != null)
            updateScore.UpdateScore(score.ToString());


        onTapableChange?.Invoke(collectable);
        Destroy(collectable.gameObject);
    }

    private void HandleTargetReached(TapAble collectable)
    {
        if (collectable is CollectableByTongue)
            (collectable as CollectableByTongue).Collect(tongueController);

        if (collectable is DragAble) { }
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

    IEnumerator takeDamageTimer(uint damage)
    {
        if (cameraObject != null)
            cameraObject.CameraShake(200);

        movementComponent.ScaleSpeed(0.25f, 1.0f);

        if (score > damage)
            score -= damage;
        else
            score = 0;

        if (updateScore != null)
            updateScore.UpdateScore(score.ToString());

        animator.SetTrigger("anim_tr_Hit");
        isGettingHurt = true;

        while (!animator.IsInTransition(0))
        {
            yield return null;
        }
        isGettingHurt = false;
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
