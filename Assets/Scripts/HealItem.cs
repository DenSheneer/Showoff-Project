using UnityEngine;

public class HealItem : DragAble
{
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
