using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Locales
{
    public static string MainLanguageKey {get{return mainLanguageKey;}}
    public static string ModLanguageKey {get{return modLanguageKey;}}
    
    public string Name {get{return name;}}

    static Locales mainLocales;
    static Locales modLocales;
    static string mainLanguageKey;
    static string modLanguageKey;

    [System.Serializable]
    struct Line{
        public string key;
        public string value;
    }
    [SerializeField] string name;
    [SerializeField] List<Line> lines = new List<Line>();
    Dictionary<string, string> lineDict = new Dictionary<string, string>();
    string langKey;

    static void SetMainKey(string newKey){
        mainLanguageKey = newKey;
        PlayerPrefs.SetString("language", newKey);
    }

    static void SetModKey(string newKey) => modLanguageKey = newKey;

    static void LoadMainLocales(){
        if(mainLocales != null) return;
        mainLanguageKey = PlayerPrefs.GetString("language", "TR");
        if(!PlayerPrefs.HasKey("language")) PlayerPrefs.SetString("language", mainLanguageKey);
        mainLocales = JsonUtility.FromJson<Locales>(Resources.Load("Locales/"+mainLanguageKey).ToString());
        mainLocales.Init(mainLanguageKey);
    }

    static void LoadModLocales(){
        if(modLocales != null) return;
        modLocales = ResourceManager.LoadAsset<Locales>(modLanguageKey);
        if(modLocales != null) modLocales.Init(modLanguageKey);
    }

    public static bool TryGetLineMod(string key, out string line, params string[] fillIns) => TryGetLineGeneric(true, key, out line, fillIns);
    public static bool TryGetLineMain(string key, out string line, params string[] fillIns) => TryGetLineGeneric(false, key, out line, fillIns);
    static bool TryGetLineGeneric(bool isMod, string key, out string line, params string[] fillIns){
        if(isMod) LoadModLocales();
        else LoadMainLocales();
        line = "";
        Locales l = isMod ? modLocales : mainLocales;
        if(l == null) return false;
        return l.TryGetLine(key, out line, fillIns);
    }

    public void Init(string langKey){
        this.langKey = langKey;
        foreach(Line l in lines){
            if(lineDict.ContainsKey(l.key)) continue;
            lineDict.Add(l.key, l.value);
        }
    }

    public bool TryGetLine(string key, out string line, params string[] fillIns){
        line = "";
        if(lineDict.TryGetValue(key, out string tempLine)){
            int fillInsIndex = 0;
            for(int i = 0; i < tempLine.Length; i++){
                char c1 = tempLine[i];
                bool has = false;
                if(i < tempLine.Length - 1){
                    char c2 = tempLine[i+1];
                    if(fillIns != null && fillInsIndex < fillIns.Length && c1 == '{' && c2== '}'){
                        line += fillIns[fillInsIndex++];
                        i++;
                        has = true;
                    }
                }
                if(!has){
                    line += c1;
                }
            }
            return true;
        }
        return false;
    }

    
}
