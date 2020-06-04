using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : CollectableByTongue
{
    int value = 1;
    public int Value { get => value; }

    TutorialIcon tutorialIcon = null;

    protected override void OnInRangeEnter()
    {
        base.OnInRangeEnter();

        tutorialIcon = new TutorialIcon(transform, TutorialType.TAB_FIREFLY);
    }
    protected override void OnInRangeStay()
    {
        base.OnInRangeStay();


        tutorialIcon.UpdateIcon();
    }
    protected override void OnExitRange()
    {
        base.OnExitRange();

        tutorialIcon.Destroy();
        tutorialIcon = null;
    }


    public override void Tab()
    {
        Debug.Log("tapped a fly");
    }
}
