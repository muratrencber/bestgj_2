using System;

public class Checker
{
    protected Func<bool> checkAction;
    protected Action<bool> onChecked;

    public Checker(Func<bool> checkAction, Action<bool> onChecked){
        this.checkAction = checkAction;
        this.onChecked = onChecked;
    }

    public virtual void Update(){
        if(checkAction == null) return;
        bool result = checkAction.Invoke();
        if(onChecked != null) onChecked.Invoke(result);
    }
}
