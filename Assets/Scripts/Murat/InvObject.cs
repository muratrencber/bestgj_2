using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvObject : InteractableBase
{
    int count;
    PlayerStorage pstrg;

    public void Set(PlayerStorage p, int count = 1){
        pstrg = p;
        this.count = count;
    }

    protected override bool EvaluateAvailability()
    {
        return pstrg;
    }

    protected override string EvaluateCursor()
    {
        return "interact";
    }

    public override void OnCursorDown()
    {
        pstrg.Add(GetComponent<Obje>().item, count);
        Destroy(gameObject);
    }

    void Update(){

    }
}
