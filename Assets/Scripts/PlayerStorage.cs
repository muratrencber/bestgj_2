using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStorage : DayEntity
{
    enum LiraType{
        BIR_LIRA,
        ELLI_KR
    }
    IDictionary<Obje, int> items = new Dictionary<Obje, int>(); 
    IDictionary<LiraType, int> liras = new Dictionary<LiraType, int>();

    public override void OnDayEnded()
    {
        throw new System.NotImplementedException();
    }
    public override void OnDayStarted()
    {
        throw new System.NotImplementedException();
    }

    float LiraToMoney(LiraType type){
        switch(type){
            case LiraType.BIR_LIRA:
                return 1;
            case LiraType.ELLI_KR:
                return 0.5f;
            default:
                return 0;
        }
    }

    float CalculateMoney() {
        float total = 0;
        foreach(KeyValuePair<LiraType, int> lira in liras){
            total += LiraToMoney(lira.Key) * lira.Value;
        }
        return total;
    }

}
