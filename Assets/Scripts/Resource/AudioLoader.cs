using System.Collections.Generic;
using UnityEngine;

public class AudioLoader
{
    public static void LoadSFX(string path, Dictionary<string, AudioClip> items){
        string[] extensions = {".wav"};
        StreamingAssetLoader<AudioClip>.Properties defaults = new StreamingAssetLoader<AudioClip>.Properties();
        StreamingAssetLoader<AudioClip> sal = new StreamingAssetLoader<AudioClip>(extensions, ProcessFile, defaults, items);
        sal.BeginLoad(path);
    }
    static AudioClip ProcessFile(string filePath,
                                string keyName,
                                StreamingAssetLoader<AudioClip>.Properties p,
                                StreamingAssetLoader<AudioClip>.PropertiesList pl,
                                StreamingAssetLoader<AudioClip> sal){
        return WavUtility.ToAudioClip(filePath);
    }
}
