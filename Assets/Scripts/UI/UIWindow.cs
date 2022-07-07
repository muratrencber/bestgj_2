using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIWindow : MonoBehaviour
{
    public string Key {get{return key;}}

    WindowList windowList;
    [SerializeField] string key;
    [SerializeField] bool setAtStart;

    public void SetWindowList(WindowList wl){
        windowList = wl;
    }

    public void Set(bool value){
        gameObject.SetActive(value);
    }
}
