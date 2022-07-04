using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager
{
    static bool loading = false;
    static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    static Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();
    static Dictionary<string, Locales> languages = new Dictionary<string, Locales>();
    static Dictionary<string, List<ColorRegion>> regions = new Dictionary<string, List<ColorRegion>>();
    static Dictionary<string, OtomatConfigs> otomats = new Dictionary<string, OtomatConfigs>();
    
    public static IEnumerator LoadGame(string path, GameObject UIObject, TMPro.TextMeshProUGUI text){
        if(loading) yield break;
        loading = true;

        float time = 0.5f;
        UIObject.SetActive(true);
        text.text = "Loading configs...";
        yield return null;
        Configs.LoadConfigurables(path+"/configs");
        yield return new WaitForSeconds(time);
        text.text = "Loading images...";
        yield return null;
        ImageLoader.LoadImages(path+"/images", sprites);
        yield return new WaitForSeconds(time);
        text.text = "Loading SFX...";
        yield return null;
        AudioLoader.LoadSFX(path+"/sfx", sfx);
        yield return new WaitForSeconds(time);
        text.text = "Loading locales...";
        yield return null;
        LocalesLoader.LoadLocales(path+"/locales", languages);
        yield return new WaitForSeconds(time);
        text.text = "Loading regions...";
        yield return null;
        ColorRegionsLoader.LoadRegions(path+"/locations", regions);
        yield return new WaitForSeconds(time);
        text.text = "Loading automatas...";
        yield return null;
        OtomatLoader.LoadOtomats(path+"/automatas", otomats);
        UIObject.SetActive(false);

        loading = false;
        yield return null;
    }

    public static T LoadAsset<T>(string path) where T:class{
        if(typeof(T) == typeof(Sprite))                     return ReturnResult<Sprite>(sprites, path) as T;
        else if(typeof(T) == typeof(AudioClip))             return ReturnResult<AudioClip>(sfx, path) as T;
        else if(typeof(T) == typeof(Locales))               return ReturnResult<Locales>(languages, path) as T;
        else if(typeof(T) == typeof(List<ColorRegion>))     return ReturnResult<List<ColorRegion>>(regions, path) as T;
        else if(typeof(T) == typeof(OtomatConfigs))     return ReturnResult<OtomatConfigs>(otomats, path) as T;
        else return null;
    }

    public static T[] LoadAllAssets<T>(string path, bool includeSubDirectories = false) where T:class{
        string[] targetKeys = new string[0];
        if(typeof(T) == typeof(Sprite)) targetKeys = sprites.Keys.ToArray();
        else if(typeof(T) == typeof(AudioClip)) targetKeys = sfx.Keys.ToArray();
        else if(typeof(T) == typeof(Locales)) targetKeys = languages.Keys.ToArray();
        else if(typeof(T) == typeof(List<ColorRegion>)) targetKeys = regions.Keys.ToArray();
        else if(typeof(T) == typeof(OtomatConfigs)) targetKeys = otomats.Keys.ToArray();
        string[] keys = targetKeys.Where((a) => (
            !includeSubDirectories ?
            a.IndexOf(path) == 0 && a.Split(path)[1].Count((c) => c == '/') <= 1 :
            a.IndexOf(path) == 0

        )).ToArray();
        List<T> result = new List<T>();
        foreach(string s in keys){
            T res = LoadAsset<T>(s);
            if(res != null) result.Add(res);
        }
        return result.ToArray();
    }

    static T ReturnResult<T>(Dictionary<string, T> dct, string path) where T:class{
        if(dct.TryGetValue(path, out T res)) return res;
        return null;
    }

    public static List<Sprite> GetAllSprites()=>sprites.Values.ToList();
    public static T[] GetAll<T>() where T:class => LoadAllAssets<T>("",true);
}
