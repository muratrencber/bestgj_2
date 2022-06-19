public class LookDisabler : InteractableBase
{
    protected override bool EvaluateAvailability()
    {
        return CameraControls.InLook;
    }
    protected override string EvaluateCursor()
    {
        return "nolook";
    }

    public override void OnCursorDown(){
        CameraControls.DisableLook();
    }
}
