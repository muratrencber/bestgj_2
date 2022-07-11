using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigruableLoader<T> where T:Configurable
{
    public static void LoadConfigs(string path, IDictionary itemsInterface){
        Dictionary<string, T> items = itemsInterface as Dictionary<string, T>;
        string[] extensions = {".json"};
        StreamingAssetLoader<T>.Properties defaults = new StreamingAssetLoader<T>.Properties();
        StreamingAssetLoader<T> sal = new StreamingAssetLoader<T>(extensions, ProcessFile, defaults, items);
        sal.BeginLoad(path);
    }
    static T ProcessFile(string filePath,
                                string keyName,
                                StreamingAssetLoader<T>.Properties p,
                                StreamingAssetLoader<T>.PropertiesList pl,
                                StreamingAssetLoader<T> sal){
        string text = System.IO.File.ReadAllText(filePath);
        T oc = JsonUtility.FromJson<T>(text);
        if(typeof(T) == typeof(Configurable)){
            System.Type targetType = System.Type.GetType(oc.ClassName);
            oc = JsonUtility.FromJson(text, targetType) as T;
        }
        oc.Init();
        return oc;
    }
}
