using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] LayerMask validLayers;

    IInteractable lastInteractable = null;
    Collider2D lastInteractableCollider = null;
    void Update(){
        bool hasOld = false;
        IInteractable newInteractable = null;
        Collider2D newCollider = null;
        Collider2D[] colls = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.01f, validLayers);
        foreach(Collider2D coll in colls){
            if(coll == lastInteractableCollider){
                hasOld = true;
                break;
            } if(newInteractable == null) {
                newCollider = coll;
                newInteractable = coll.GetComponent<IInteractable>();
                if(lastInteractable == null)
                    break;
            }
        }

        if(!hasOld && newInteractable != null){
            CursorManager.SetDefault();
            if(lastInteractable != null){
                lastInteractable.OnMouseExit();
            }
            lastInteractable = newInteractable;
            lastInteractableCollider = newCollider;
            lastInteractable.OnMouseEnter();
            CursorManager.Set(lastInteractable.cursorOverride);
        } else if(hasOld){
            lastInteractable.OnMouseStay();
        } else{
            CursorManager.SetDefault();
            if(lastInteractable != null){
                lastInteractable.OnMouseExit();
            }
            lastInteractable = null;
            lastInteractableCollider = null;
        }

        if(lastInteractable != null){
            if(Input.GetMouseButtonDown(0))
                lastInteractable.OnMouseDown();
            else if(Input.GetMouseButton(0))
                lastInteractable.OnMouseHold();
            else if(Input.GetMouseButtonUp(0))
                lastInteractable.OnMouseUp();
        }
    }
}
