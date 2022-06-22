using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInsert : InteractableBase
{
    [SerializeField] Otomat o;

    protected override bool EvaluateAvailability(){
        return o.CanAcceptMoney() && PlayerStorage.instance.HasMoney;
    }

    protected override string EvaluateCursor(){
        return "coin";
    }

    public override void OnCursorDown(){
        o.ReceiveMoney();
    }
}
