using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Locales
{
    [System.Serializable]
    struct Line{
        public string key;
        public string value;
    }
    [SerializeField] string name;
    [SerializeField] List<Line> lines = new List<Line>();
    Dictionary<string, string> lineDict = new Dictionary<string, string>();
    string langKey;

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
            for(int i = 0; i < tempLine.Length - 1; i++){
                char c1 = tempLine[i];
                char c2 = tempLine[i+1];
                if(fillIns != null && fillInsIndex < fillIns.Length && c1 == '{' && c2== '}'){
                    line += fillIns[fillInsIndex++];
                    i++;
                } else {
                    line += c1;
                }
            }
            return true;
        }
        return false;
    }

    
}
