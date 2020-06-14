using UnityEngine;

public class HealItem : CollectableByTongue
{
    int healAmount = 1;

    public int HealAmount { get => healAmount; }

    public override void Tab()
    {
        Debug.Log("tapped a healItem");
    }
}
