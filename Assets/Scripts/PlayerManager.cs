using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TongueController))]
[RequireComponent(typeof(FollowRaycastNavMesh))]
public class PlayerManager : MonoBehaviour
{
    private static AudioSource audioSource;

    private FollowRaycastNavMesh movementComponent = null;
    private TongueController tongueController;

    public delegate void OnTapableChange(TapAble changedTapable);
    public delegate void IsBeingDamaged();

    public OnTapableChange onTapableChange;
    public IsBeingDamaged isBeingDamaged;

    public Action<int> OnFireflyChange;
    public Action<int> OnScoreChange;

    private static Animator animator = null;
    private static ParticleSystem particles_beetle = null;
    private static ParticleSystem particles_fly = null;

    private Camerashake cameraObject;

    [SerializeField]
    private int nrOfFlies = 0, nrOfBeetles = 0;
    private int score = 0;
    public readonly int MaxNrOfFlies = 3;

    private bool isGettingHurt = false;

    private Dictionary<InputType, bool> tutorials = new Dictionary<InputType, bool>();
    private TutorialIcon tutorialIcon;
    private TapAble nearbyTapAble = null;
    private readonly float hintIdleTime = 5.0f;
    private float hintTimer = 0.0f;     // --> Do not change
    private bool extraHintActive;

    public Vector3 Position { get => transform.position; }
    public int NrOfFlies { get => nrOfFlies; }
    public int Score { get => score; }
    public bool IsMoving { get => movementComponent.IsMoving; }

    public bool IsBusy()
    {
        if (!isGettingHurt)
            return tongueController.InProgress;
        else
            return isGettingHurt;
    }

    private void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
        particles_beetle = GameObject.Find("PickupEffect").GetComponent<ParticleSystem>();
        particles_fly = GameObject.Find("PickupFirefly").GetComponent<ParticleSystem>();
        movementComponent = GetComponent<FollowRaycastNavMesh>();
        tongueController = GetComponentInChildren<TongueController>();
        cameraObject = Camera.main.GetComponent<Camerashake>();

        tongueController.tongueReachedTarget += HandleTargetReached;
        tongueController.targetEaten += HandleTargetEaten;

        audioSource = GetComponent<AudioSource>();

        isBeingDamaged += cameraShake;
        isBeingDamaged += slowMovement;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnScoreChange?.Invoke(score);
        }

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
                    playSound(AudioData.TongueSound);
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
                        updateIntValue(ref nrOfFlies, -1, OnFireflyChange);
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
            Debug.Log(tongueController.tongueReachedTarget.GetInvocationList().Length);
            animator.SetBool("anim_isOpen", false);
            tongueController.DetacheDragAble(tapAble as DragAble);
            movementComponent.reverseDirection = false;
            return;
        }
    }
    void handleCollectable(CollectableByTongue collectable)
    {
        tongueController.SetCollectTarget(collectable);
    }

    public void takeDamage(int damage)
    {
        playSound(AudioData.Hurt);
        updateIntValue(ref score, -damage, OnScoreChange);
        StartCoroutine(takeDamageTimer(damage));
    }

    private void HandleTargetEaten(TapAble collectable)
    {
        animator.SetBool("anim_isOpen", false);

        if (collectable is Fly)
        {
            Fly fly = collectable as Fly;

            if (particles_fly != null)
                particles_fly.Play();

            if (nrOfFlies < 3)
                updateIntValue(ref nrOfFlies, fly.Value, OnFireflyChange);
            else
                updateIntValue(ref score, 10, OnScoreChange);
        }
        if (collectable is Beetle)
        {
            Beetle beetle = collectable as Beetle;
            if (particles_beetle != null)
                particles_beetle.Play();

            updateIntValue(ref score, beetle.Value, OnScoreChange);
            updateIntValue(ref nrOfBeetles, 1);
        }

        onTapableChange?.Invoke(collectable);
        Destroy(collectable.gameObject);
    }

    private void HandleTargetReached(TapAble collectable)
    {
        if (collectable is CollectableByTongue)
            (collectable as CollectableByTongue).Collect(tongueController);
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

    IEnumerator takeDamageTimer(int damage)
    {
        isGettingHurt = true;
        animator.SetTrigger("anim_tr_Hit");
        isBeingDamaged?.Invoke();

        while (!animator.IsInTransition(0))
        {
            yield return null;
        }
        isGettingHurt = false;
    }

    void playSound(string name)
    {
        if (audioSource != null)
        {
            AudioClip clip = Resources.Load<AudioClip>(AudioData.path + name);
            audioSource.PlayOneShot(clip);
        }
    }
    void cameraShake()
    {
        if (cameraObject != null)
            cameraObject.CameraShake(200);
    }
    void slowMovement()
    {
        movementComponent.ScaleSpeed(0.25f, 1.0f);
    }

    void updateIntValue(ref int targetVariable, int addedValue, Action<int> pEvent = null)
    {
        if (addedValue > 0)
            targetVariable += addedValue;

        else if (targetVariable >= Math.Abs(addedValue))
            targetVariable = targetVariable + addedValue;

        else if (targetVariable < addedValue)
            targetVariable = 0;

        pEvent?.Invoke(targetVariable);
    }

    public void AutoMove(Vector3 destination)
    {
        movementComponent.SetDestination(destination);
    }
}
