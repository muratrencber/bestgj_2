using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIWindow : MonoBehaviour
{

    [SerializeField] string key;
    [SerializeField] bool setAtStart;
    public static void ChangeToWindow(string key){
        UIWindow[] windows = GameObject.FindObjectsOfType<UIWindow>(true);
        var contenders = windows.Where((a) => a.key == key);
        if(contenders != null && contenders.Count() > 0)
            contenders.First().SetActive();
    }

    public void Set(bool value){
        gameObject.SetActive(value);
    }

    public void SetActive(){
        foreach(UIWindow uw in GameObject.FindObjectsOfType<UIWindow>(true)){
            uw.Set(uw == this);
        }
    }
}
