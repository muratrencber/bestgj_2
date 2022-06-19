using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInsert : ItemReceiver
{
    [SerializeField] Otomat o;
    [SerializeField] PlayerStorage pstrg;

    GameObject target;
    public override void TakeItem(GameObject itemObject)
    {
        pstrg.RemoveItem(Items.BIR_LIRA, 1);
        target = itemObject;
        //DestroyImmediate(target.GetComponent<DraggableObject>());
        Destroy(target);
        //Invoke("DestroyIbject", 0.01f);
        o.enterLira();
    }

    //void DestroyIbject() => DestroyImmediate(target);
}
