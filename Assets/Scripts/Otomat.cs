using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Otomat : DayEntity
{
    int currency;
    int currentNumber;
    Obje[] items;

    void GiveItem(int no) {
        
        Obje item = items[no];
        

        GiveChange();
    }

    void GiveChange() {


    }
    public override void OnDayEnded()
    {
        throw new System.NotImplementedException();
    }
    public override void OnDayStarted()
    {
        throw new System.NotImplementedException();
    }


}
