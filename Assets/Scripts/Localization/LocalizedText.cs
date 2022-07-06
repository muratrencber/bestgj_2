using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] string targetKey;
    [SerializeField] string[] fillIns;
    [SerializeField] bool isMod;

    void Awake(){
        Set();
    }

    public void Set(){
        string s = "";
        if(isMod) Locales.TryGetLineMod(targetKey, out s, fillIns);
        else Locales.TryGetLineMain(targetKey, out s, fillIns);
        GetComponent<TMPro.TextMeshProUGUI>().text = s;
    }
}
