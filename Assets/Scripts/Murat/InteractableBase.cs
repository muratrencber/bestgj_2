using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected string cursorName;
    public virtual void OnMouseEnter(){

    }
    public virtual void OnMouseStay(){

    }
    public virtual void OnMouseExit(){

    }
    public virtual void OnMouseDown(){

    }
    public virtual void OnMouseHold(){

    }
    public virtual void OnMouseUp(){

    }
    public string cursorOverride {get{
        return cursorName;
    }}
}
