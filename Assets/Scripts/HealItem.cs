using UnityEngine;

public class HealItem : DragAble
{
    int healAmount = 1;

    public int HealAmount { get => healAmount; }
    public override void InRange()
    {
        throw new System.NotImplementedException();
    }

    public override void OutOfRange()
    {
        throw new System.NotImplementedException();
    }

    public override void Tab()
    {
        gameObject.SetActive(false);
        Debug.Log("tapped a healItem");
    }
}
