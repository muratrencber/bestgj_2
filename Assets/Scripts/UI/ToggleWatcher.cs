using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWatcher : Checker
{
    public ToggleWatcher(List<Toggle> targets, bool enableWhenToggled, GameObject targetObject) : base(
        () => {
            bool isOn = true;
            foreach(Toggle t in targets){
                if(!t.isOn){
                    isOn = false;
                    break;
                }
            }
            return isOn;
        },
        (isOn) => {
            if(!enableWhenToggled) isOn = !isOn;
            if(isOn != targetObject.activeSelf)
                targetObject.SetActive(!targetObject.activeSelf);
        }
    ){}
}
