using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSButton : InteractableBase
{
    public PlayerStorage pstrg;
    public int count;

    protected override bool EvaluateAvailability()
    {
        return gameObject.activeInHierarchy && enabled;
    }

    protected override string EvaluateCursor()
    {
        return "interact";
    }

    public override void OnCursorDown()
    {
        pstrg.SetItemAway(count);
    }
}
