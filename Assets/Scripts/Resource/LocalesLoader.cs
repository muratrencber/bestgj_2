using System.Collections.Generic;
using UnityEngine;

public class LocalesLoader
{
    public static void LoadLocales(string path, Dictionary<string, Locales> items){
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
        return JsonUtility.FromJson<Locales>(System.IO.File.ReadAllText(filePath));
    }
}
