public interface IInteractable
{
    void OnMouseEnter();
    void OnMouseStay();
    void OnMouseExit();
    void OnMouseDown();
    void OnMouseHold();
    void OnMouseUp();
    string cursorOverride {get;}
}
