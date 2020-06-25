public class Beetle : CollectableByTongue
{
    private void Start()
    {
        base.Start();

        //  base class values.
        tapAbleType = InputType.TAP_BEETLE;
        maxScaleFactor = 1.5f;
        scaleSpeed = 0.75f;
        value = 5;
    }

    public override void Tab()
    {
        return;
    }
}
