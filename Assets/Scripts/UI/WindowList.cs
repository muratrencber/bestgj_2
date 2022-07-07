using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WindowList : MonoBehaviour
{
    [SerializeField] List<UIWindow> windows = new List<UIWindow>();
    [SerializeField] string startWindowKey;
    string currentKey;

    void Awake(){
        TrySetWindow(startWindowKey);
    }

    public void SetWindow(string wKey){
        foreach(UIWindow uw in windows)
            uw.Set(uw.Key == wKey);
        currentKey = wKey;
    }

    bool ContainsWindow(string key) => windows.Where((uw)=>uw.Key == key).Count() > 0;

    public bool TrySetWindow(string key){
        if(ContainsWindow(key)){
            SetWindow(key);
            return true;
        }
        return false;
    }
}
