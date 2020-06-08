using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableByTongue : TapAble
{
    [SerializeField]
    private int collectingWeight = 1;

    // InRange shrink-expand variables:
    protected float minScaleFactor = 1.0f, maxScaleFactor = 2.0f, scaleSpeed = 1.00f;
    protected Vector3 originalScale;
    FloatingBehaviour floatingBehaviour;

    [SerializeField]
    Transform GFXTransform;

    public int CollectingWeight { get => collectingWeight; }
    public Vector3 Position { get => transform.position; }

    void OnEnable()
    {
        this.tag = "TongueCollectable";
        originalScale = GFXTransform.localScale;

        EnterEvent += collectable_OnInRange;
        EnterStayEvent += collectable_InRangeStay;
        ExitEvent += collectable_OnExit;
    }

    public void Collect(TongueController collector)
    {
        transform.parent = collector.transform;
    }

    void collectable_OnExit(TapAble tapAble)
    {
        GFXTransform.localScale = originalScale;
        floatingBehaviour = null;
    }

    private void collectable_OnInRange(TapAble tapAble)
    {
        floatingBehaviour = new FloatingBehaviour(scaleSpeed, minScaleFactor, maxScaleFactor);
    }
    private void collectable_InRangeStay(TapAble tapAble)
    {
        GFXTransform.localScale = originalScale * floatingBehaviour.GetScaleFactor();
    }

    public static void Disable(CollectableByTongue collectable)
    {
        collectable.OutOfRange();
        collectable.gameObject.SetActive(false);
    }
}
