using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GameLoader
{
    const string path = "games";
    [System.Serializable]
    public class Properties{
        public string GameName {get{return gameName;}}
        public string GamePath {get{return gamePath;}}
        public string Language {get{return defaultLanguage;}}

        [SerializeField] string gameName;
        [SerializeField] string defaultLanguage;
        string gamePath;

        public void SetPath(string p){
            gamePath = p;
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
            allGames.Add(props);
        }
        return allGames.ToArray();
    }
}
