using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationArea : InteractableBase
{
    public string key;
    protected override string EvaluateCursor()
    {
        return "go";
    }

    protected override bool EvaluateAvailability()
    {
        Location loc = Location.GetLocation(key);
        return loc && !CameraControls.InLook;
    }

    public override void OnCursorDown(){
        UIPrompt.Command c = new UIPrompt.Command();
        c.bgFadeDuration = 0.1f;
        c.onFinished = ChangeLocation;

        UIPrompt.AddCommand(c);
        UIPrompt.StartEvaluating();
    }

    void ChangeLocation(){
        Location loc = Location.GetLocation(key);
        CameraControls.Move(loc.transform.position);
    }
}
