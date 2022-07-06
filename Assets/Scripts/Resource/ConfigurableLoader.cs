using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigruableLoader<T> where T:Configurable
{
    public static void LoadConfigs(string path, Dictionary<string, T> items){
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
        T oc = JsonUtility.FromJson<T>(System.IO.File.ReadAllText(filePath));
        oc.Init();
        return oc;
    }
}
