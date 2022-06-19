using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected string cursorName;
    protected bool isAvailable = true;
    public virtual void OnCursorEnter(){

    }
    public virtual void OnCursorStay(){

    }
    public virtual void OnCursorExit(){

    }
    public virtual void OnCursorDown(){

    }
    public virtual void OnCursorHold(){

    }
    public virtual void OnCursorUp(){

    }
    public virtual void OnChangedInteractable(){
        
    }
    protected virtual string EvaluateCursor(){
        return cursorName;
    }
    protected virtual bool EvaluateAvailability(){
        return isAvailable;
    }
    public string cursorOverride {get{
        return EvaluateCursor();
    }}
    public bool canBeInteracted {get{return EvaluateAvailability();}}
}
