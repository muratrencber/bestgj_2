using UnityEngine;
using System.Collections.Generic;

public class LookArea : InteractableBase
{
    public float newCameraSize;
    public Transform newCameraPosition;
    [SerializeField] List<GameObject> disablers = new List<GameObject>();

    void Start(){
        SetDisablers(false);
    }

    protected override bool EvaluateAvailability()
    {
        return !CameraControls.InLook;
    }
    protected override string EvaluateCursor()
    {
        return "look";
    }

    public void EndedLook(){
        SetDisablers(false);
    }

    public override void OnCursorDown(){
        CameraControls.SetLook(this);
        SetDisablers(true);
    }

    void SetDisablers(bool value){
        foreach(GameObject gj in disablers)
            gj.SetActive(value);
    }
}
