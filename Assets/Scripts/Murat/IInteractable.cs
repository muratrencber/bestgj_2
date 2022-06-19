using UnityEngine;
public interface IInteractable
{
    public GameObject gameObject {get;}
    void OnCursorEnter();
    void OnCursorStay();
    void OnCursorExit();
    void OnCursorDown();
    void OnCursorHold();
    void OnCursorUp();
    void OnChangedInteractable();
    bool canBeInteracted {get;}
    string cursorOverride {get;}
}
