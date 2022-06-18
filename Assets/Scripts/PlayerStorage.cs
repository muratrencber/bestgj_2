using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStorage : DayEntity
{

    IDictionary<Obje, int> items = new Dictionary<Obje, int>(); 
    IDictionary<string, int> liras = new Dictionary<string, int>();

    public override void OnDayEnded()
    {
        throw new System.NotImplementedException();
    }
    public override void OnDayStarted()
    {
        throw new System.NotImplementedException();
    }

    float CalculateMoney() {

        
    }

}
