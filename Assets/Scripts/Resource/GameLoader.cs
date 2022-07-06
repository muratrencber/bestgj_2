using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameLoader
{
    const string path = "games";
    [System.Serializable]
    public class Properties{
        public string DisplayName{get{return displayName;}}
        public string GameName {get{return gameName;}}
        public string GamePath {get{return gamePath;}}
        public string Language {get{return defaultLanguage;}}

        [SerializeField] string gameName;
        [SerializeField] string defaultLanguage;
        string gamePath;
        string displayName;

        public void SetPath(string p){
            gamePath = p;
        }

        public void SetTitle(){
            displayName = gameName;
            Dictionary<string, Locales> locales = new Dictionary<string, Locales>();
            try{
                LocalesLoader.LoadLocales(gamePath+"/locales", locales);
            }catch(System.Exception e){
                return;
            }
            Locales selectedLocales = null;
            if(Locales.MainLanguageKey != null && locales.TryGetValue(Locales.MainLanguageKey, out Locales res1)){
                selectedLocales = res1;
            } else if(defaultLanguage != null && locales.TryGetValue(defaultLanguage, out Locales res2)){
                selectedLocales = res2;
            } else if(locales.Count > 0){
                selectedLocales = locales.Values.FirstOrDefault();
            }
            if(selectedLocales != null){
                if(selectedLocales.TryGetLine(gameName, out string res)){
                    displayName = res;
                }
            }
        }
    }

    public static Properties[] LoadGames(){
        string mainPath = string.Format("{0}/{1}", Application.streamingAssetsPath, path);
        if(!Directory.Exists(mainPath)) return new Properties[0];
        string[] directories = Directory.GetDirectories(mainPath);
        List<Properties> allGames = new List<Properties>();
        foreach(string checkPath in directories){
            string propsPath = checkPath + "/properties.json";
            if(!File.Exists(propsPath)) continue;
            string contents = File.ReadAllText(propsPath);
            Properties props = JsonUtility.FromJson<Properties>(contents);
            props.SetPath(checkPath);
            props.SetTitle();
            allGames.Add(props);
        }
        return allGames.ToArray();
    }
}
