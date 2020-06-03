using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableByTongue : TapAble
{
    [SerializeField]
    private int collectingWeight = 1;

    // InRange shrink-expand variables:
    protected float minScaleFactor = 1.0f, maxScaleFactor = 2.0f, scaleSpeed = 1.00f;
    Vector3 originalScale;
    FloatingBehaviour floatingBehaviour;

    public int CollectingWeight { get => collectingWeight; }
    public Vector3 Position { get => transform.position; }

    void OnEnable()
    {
        originalScale = transform.localScale;
        this.tag = "TongueCollectable";
    }

    public void Collect(TongueController collector)
    {
        transform.localScale = originalScale;
        transform.parent = collector.transform;
        originalScale = transform.localScale;
    }

    protected override void OnInRangeEnter()
    {
        floatingBehaviour = new FloatingBehaviour(scaleSpeed, minScaleFactor, maxScaleFactor);
    }
    protected override void OnInRangeStay()
    {
        transform.localScale = originalScale * floatingBehaviour.GetScaleFactor();
        return;
    }
    protected override void OnExitRange()
    {
        gameObject.transform.localScale = originalScale;
        floatingBehaviour = null;
    }
    protected override void OnOutOfRangeStay()
    {
        return;
    }

    public static void Disable(CollectableByTongue collectable)
    {
        collectable.OutOfRange();
        collectable.gameObject.SetActive(false);
    }
}
