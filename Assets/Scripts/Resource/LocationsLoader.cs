using System.Collections.Generic;
using UnityEngine;

public class LocationProperties{
    public string locationKey;
    public string background;
    public string cameraColor;
}

public class LocationsLoader{

    
    public static void LoadLocations(string path, Dictionary<string, LocationProperties> items){
        string[] extensions = {".loc"};
        StreamingAssetLoader<LocationProperties>.Properties defaults = new StreamingAssetLoader<LocationProperties>.Properties();
        StreamingAssetLoader<LocationProperties> sal = new StreamingAssetLoader<LocationProperties>(extensions, ProcessFile, defaults, items);
        sal.BeginLoad(path);
    }
    static LocationProperties ProcessFile(string filePath,
                                string keyName,
                                StreamingAssetLoader<LocationProperties>.Properties p,
                                StreamingAssetLoader<LocationProperties>.PropertiesList pl,
                                StreamingAssetLoader<LocationProperties> sal){
        return JsonUtility.FromJson<LocationProperties>(System.IO.File.ReadAllText(filePath));
    }
}
