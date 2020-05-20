using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableByTongue : TapAble
{
    [SerializeField]
    private int collectingWeight = 1;
    public int CollectingWeight { get => collectingWeight; }
    public Vector3 Position { get => transform.position; }

    FloatingBehaviour floating;
    void Start()
    {
        floating = new FloatingBehaviour(0.2f, 0.3f, 0.15f);
        this.tag = "TongueCollectable";
    }
    public override void InRange()
    {
        floating.Update();
        gameObject.transform.localScale = new Vector3(floating.result, floating.result, floating.result);
    }
    public override void OutOfRange()
    {
        gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        return;
    }
}
