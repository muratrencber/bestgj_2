using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] LayerMask validLayers;

    IInteractable lastInteractable = null;
    Collider2D lastInteractableCollider = null;
    bool Check(IInteractable inter){
        return lastInteractableCollider;
    }
    void Update(){
        if(UIPrompt.Busy || PlayerStorage.instance.gameEnded)
            return;
        if(Check(lastInteractable) && !lastInteractable.canBeInteracted){
            lastInteractable = null;
            lastInteractableCollider = null;
        }
        bool hasOld = false;
        IInteractable newInteractable = null;
        Collider2D newCollider = null;
        Collider2D[] colls = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.05f, validLayers);
        foreach(Collider2D coll in colls){
            if(coll == lastInteractableCollider){
                hasOld = true;
                break;
            } if(newInteractable == null) {
                newCollider = coll;
                IInteractable[] ints = coll.GetComponents<IInteractable>();
                if(ints != null){
                    foreach(IInteractable i in ints){
                        if(i.canBeInteracted){
                            newInteractable = i;
                            break;
                        }
                    }
                }
                if(newInteractable != null && !newInteractable.canBeInteracted){
                    newInteractable = null;
                    newCollider = null;
                }
                if(lastInteractable == null && newInteractable != null)
                    break;
            }
        }
        if(!hasOld && newInteractable != null && !DraggableObject.IsDragging){
            CursorManager.SetDefault();
            if(lastInteractable != null){
                lastInteractable.OnCursorExit();
                lastInteractable.OnChangedInteractable();
            }
            lastInteractable = newInteractable;
            lastInteractableCollider = newCollider;
            lastInteractable.OnCursorEnter();
            CursorManager.Set(lastInteractable.cursorOverride);
        } else if(hasOld || DraggableObject.IsDragging){
            lastInteractable.OnCursorStay();
        } else{
            CursorManager.SetDefault();
            if(lastInteractable != null){
                lastInteractable.OnCursorExit();
            }
            lastInteractable = null;
            lastInteractableCollider = null;
        }

        if(lastInteractable != null){
            if(Input.GetMouseButtonDown(0))
                lastInteractable.OnCursorDown();
            else if(Input.GetMouseButton(0))
                lastInteractable.OnCursorHold();
            else if(Input.GetMouseButtonUp(0))
                lastInteractable.OnCursorUp();
        }
    }
}
