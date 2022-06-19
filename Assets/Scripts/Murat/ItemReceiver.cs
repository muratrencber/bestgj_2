using UnityEngine;

public class ItemReceiver : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] bool takesPara;
    public bool Check(GameObject other, Items i){
        if(takesPara != (i == Items.BIR_LIRA)){
            return false;
        }
        return Vector2.Distance(transform.position, other.transform.position) <= range;
    }
    public virtual void TakeItem(GameObject itemObject){

    }
}
