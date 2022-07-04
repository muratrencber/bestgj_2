using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtomatLoader
{
    public static void LoadOtomats(string path, Dictionary<string, OtomatConfigs> items){
        string[] extensions = {".json"};
        StreamingAssetLoader<OtomatConfigs>.Properties defaults = new StreamingAssetLoader<OtomatConfigs>.Properties();
        StreamingAssetLoader<OtomatConfigs> sal = new StreamingAssetLoader<OtomatConfigs>(extensions, ProcessFile, defaults, items);
        sal.BeginLoad(path);
    }
    static OtomatConfigs ProcessFile(string filePath,
                                string keyName,
                                StreamingAssetLoader<OtomatConfigs>.Properties p,
                                StreamingAssetLoader<OtomatConfigs>.PropertiesList pl,
                                StreamingAssetLoader<OtomatConfigs> sal){
        OtomatConfigs oc = JsonUtility.FromJson<OtomatConfigs>(System.IO.File.ReadAllText(filePath));
        oc.Init();
        return oc;
    }
}
