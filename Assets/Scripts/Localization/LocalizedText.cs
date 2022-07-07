using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] string targetKey;
    [SerializeField] string[] fillIns;
    [SerializeField] bool isMod;

    void OnEnable(){
        Display();
    }
    public void Display(){
        if(targetKey == null) return;
        string s = "";
        if(isMod) Locales.TryGetLineMod(targetKey, out s, fillIns);
        else Locales.TryGetLineMain(targetKey, out s, fillIns);
        GetComponent<TMPro.TextMeshProUGUI>().text = s;
    }
    public void Set(string key, bool isMod, params string[] fillIns){
        this.targetKey = key;
        this.isMod = isMod;
        this.fillIns = fillIns == null ? new string[0] : fillIns;
        Display();
    }
}
