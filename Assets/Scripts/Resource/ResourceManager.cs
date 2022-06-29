using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager
{
    static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    static Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();
    static Dictionary<string, Locales> languages = new Dictionary<string, Locales>();
    static Dictionary<string, List<ColorRegion>> regions = new Dictionary<string, List<ColorRegion>>();

    public static void LoadGame(string path){
        ImageLoader.LoadImages(path+"/images", sprites);
        AudioLoader.LoadSFX(path+"/sfx", sfx);
        LocalesLoader.LoadLocales(path+"/locales", languages);
        ColorRegionsLoader.LoadRegions(path+"/locations", regions);
    }

    public static T LoadAsset<T>(string path) where T:class{
        if(typeof(T) == typeof(Sprite)){
            if(sprites.TryGetValue(path, out Sprite result))
                return result as T;
            return null;
        } else if(typeof(T) == typeof(AudioClip)){
            if(sfx.TryGetValue(path, out AudioClip result))
                return result as T;
            return null;
        } else return null;
    }

    public static List<Sprite> GetAllSprites()=>sprites.Values.ToList();
}
