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
        originalScale = GFXTransform.localScale;
        this.tag = "TongueCollectable";
    }

    public void Collect(TongueController collector)
    {
        transform.parent = collector.transform;
        floatingBehaviour = null;
    }

    protected override void OnInRangeEnter()
    {
        floatingBehaviour = new FloatingBehaviour(scaleSpeed, minScaleFactor, maxScaleFactor);
    }
    protected override void OnInRangeStay()
    {
        if (floatingBehaviour != null)
            GFXTransform.localScale = originalScale * floatingBehaviour.GetScaleFactor();
        return;
    }
    protected override void OnExitRange()
    {
        GFXTransform.localScale = originalScale;
        floatingBehaviour = null;
    }
    protected override void OnOutOfRangeStay()
    {
        return;
    }


    public Transform GetGFXTransform()
    {
        return GFXTransform;
    }

    public static void Disable(CollectableByTongue collectable)
    {
        collectable.OutOfRange();
        collectable.gameObject.SetActive(false);
    }
}
