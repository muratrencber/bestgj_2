using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWatcher : MonoBehaviour
{
    [SerializeField] List<Toggle> targets = new List<Toggle>();
    [SerializeField] bool enableWhenToggled = true;
    [SerializeField] GameObject targetObject;

    void Update(){
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
