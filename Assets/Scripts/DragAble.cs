using UnityEngine;
public class DragAble : TapAble
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
        Debug.Log("tabbed a dragAble");
    }
}
