using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvObject : InteractableBase
{
    [SerializeField] bool isLira = false;
    [SerializeField] int count;

    public void Set(int count = 1){
        this.count = count;
    }

    protected override string EvaluateCursor(){
        return "interact";
    }

    public override void OnCursorDown(){
        if(!isLira) PlayerStorage.instance.AddItem(GetComponent<Obje>().item, count);
        else PlayerStorage.instance.AddCoin(count);
        Destroy(gameObject);
    }
}
