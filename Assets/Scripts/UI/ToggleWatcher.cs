using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWatcher
{
    List<Toggle> targets = new List<Toggle>();
    bool enableWhenToggled = true;
    GameObject targetObject;

    public ToggleWatcher(List<Toggle> targets, bool enableWhenToggled, GameObject targetObject){
        if(targets != null) this.targets = targets;
        this.enableWhenToggled = enableWhenToggled;
        this.targetObject = targetObject;
    }

    public void Update(){
        bool isOn = true;
        foreach(Toggle t in targets){
            if(!t.isOn){
                isOn = false;
                break;
            }
        }
        if(!enableWhenToggled) isOn = !isOn;
        if(isOn != targetObject.activeSelf)
            targetObject.SetActive(!targetObject.activeSelf);
    }
}
