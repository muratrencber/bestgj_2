using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalesLoader
{
    public static void LoadLocales(string path, IDictionary itemsInterface){
        Dictionary<string, Locales> items = itemsInterface as Dictionary<string, Locales>;
        string[] extensions = {".lang"};
        StreamingAssetLoader<Locales>.Properties defaults = new StreamingAssetLoader<Locales>.Properties();
        StreamingAssetLoader<Locales> sal = new StreamingAssetLoader<Locales>(extensions, ProcessFile, defaults, items);
        sal.BeginLoad(path);
    }
    static Locales ProcessFile(string filePath,
                                string keyName,
                                StreamingAssetLoader<Locales>.Properties p,
                                StreamingAssetLoader<Locales>.PropertiesList pl,
                                StreamingAssetLoader<Locales> sal){
        Locales l = JsonUtility.FromJson<Locales>(System.IO.File.ReadAllText(filePath));
        l.Init(keyName);
        return l;
    }
}
