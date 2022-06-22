using UnityEngine;
using System.Linq;
public class MouseController : MonoBehaviour
{
    [SerializeField] LayerMask validLayers;

    IInteractable lastInteractable = null;
    Collider2D lastInteractableCollider = null;
    void Update(){
        if(UIPrompt.Busy)
            return;
        if(lastInteractableCollider && !lastInteractable.canBeInteracted){
            lastInteractable = null;
            lastInteractableCollider = null;
        }
        IInteractable newInteractable = null;
        Collider2D newCollider = null;
        Collider2D[] colls = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.05f, validLayers);
        foreach(Collider2D coll in colls){
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
            if(newInteractable != null)
                break;
        }
        if(newInteractable != null && !DraggableObject.IsDragging){
            CursorManager.SetDefault();
            if(lastInteractableCollider){
                lastInteractable.OnCursorExit();
            } if(lastInteractableCollider){
                lastInteractable.OnChangedInteractable();
            }
            lastInteractable = newInteractable;
            lastInteractableCollider = newCollider;
            lastInteractable.OnCursorEnter();
            CursorManager.Set(lastInteractable.cursorOverride);
        } else if(DraggableObject.IsDragging){
            lastInteractable.OnCursorStay();
        } else{
            CursorManager.SetDefault();
            if(lastInteractableCollider){
                lastInteractable.OnCursorExit();
            }
            lastInteractable = null;
            lastInteractableCollider = null;
        }

        if(lastInteractableCollider){
            if(Input.GetMouseButtonDown(0))
                lastInteractable.OnCursorDown();
            else if(Input.GetMouseButton(0))
                lastInteractable.OnCursorHold();
            else if(Input.GetMouseButtonUp(0))
                lastInteractable.OnCursorUp();
        }
    }
}
