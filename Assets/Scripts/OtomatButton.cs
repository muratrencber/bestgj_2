using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtomatButton : InteractableBase
{
    [SerializeField] public int number;
    [SerializeField] Animator animator;
    [SerializeField] Otomat otomat;
    protected override string EvaluateCursor()
    {
        return "interact";
    }

    protected override bool EvaluateAvailability()
    {
        return otomat.CanAcceptInput();
    }

    public override void OnCursorDown()
    {
        animator.SetTrigger("click");
        otomat.PressKey(number);
    }
}
