using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ResourceManager
{
    static bool loading = false;
    static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    static Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();
    static Dictionary<string, Locales> languages = new Dictionary<string, Locales>();
    static Dictionary<string, List<ColorRegion>> regions = new Dictionary<string, List<ColorRegion>>();
    static Dictionary<string, OtomatConfigs> otomats = new Dictionary<string, OtomatConfigs>();
    static Dictionary<string, StudentConfigs> students = new Dictionary<string, StudentConfigs>();
    static Dictionary<string, LocationProperties> locations = new Dictionary<string, LocationProperties>();

    static void ClearAll(){
        sprites.Clear();
        sfx.Clear();
        languages.Clear();
        regions.Clear();
        otomats.Clear();
        students.Clear();
        locations.Clear();
    }
    
    public static IEnumerator LoadGame(string path, GameObject UIObject, TMPro.TextMeshProUGUI text, Action<Exception> exceptionHandler, Action onFinished){
        if(loading) yield break;
        loading = true;

        ClearAll();
        float time = 0.5f;
        UIObject.SetActive(true);

        yield return SetPrompt(text, "loading_configs", time);
        if(!TryLoad(() => Configs.LoadConfigurables(path + "/configs"), exceptionHandler)) yield break;
        
        yield return SetPrompt(text, "loading_images", time);
        if(!TryLoad(() => ImageLoader.LoadImages(path+"/images", sprites), exceptionHandler)) yield break;

        yield return SetPrompt(text, "loading_sfx", time);
        if(!TryLoad(() => AudioLoader.LoadSFX(path+"/sfx", sfx), exceptionHandler)) yield break;

        yield return SetPrompt(text, "loading_locales", time);
        if(!TryLoad(() => LocalesLoader.LoadLocales(path+"/locales", languages), exceptionHandler)) yield break;

        yield return SetPrompt(text, "loading_regions", time);
        if(!TryLoad(() => ColorRegionsLoader.LoadRegions(path+"/locations", regions), exceptionHandler)) yield break;

        yield return SetPrompt(text, "loading_automata", time);
        if(!TryLoad(() => ConfigruableLoader<OtomatConfigs>.LoadConfigs(path+"/automatas", otomats), exceptionHandler)) yield break;

        yield return SetPrompt(text, "loading_custom_students", time);
        if(!TryLoad(() => ConfigruableLoader<StudentConfigs>.LoadConfigs(path+"/students", students), exceptionHandler)) yield break;

        yield return SetPrompt(text, "loading_locations", time);
        if(!TryLoad(() => LocationsLoader.LoadLocations(path+"/locations", locations), exceptionHandler)) yield break;

        UIObject.SetActive(false);
        loading = false;

        onFinished?.Invoke();
        yield return null;
    }

    static IEnumerator SetPrompt(TMPro.TextMeshProUGUI text, string s, float seconds){
        Locales.TryGetLineMain(s, out string locS);
        text.text = locS;
        yield return new WaitForSeconds(seconds);
    }

    static bool TryLoad(Action method, Action<Exception> exceptionHandler){
        try{
            method.Invoke();
        } catch(Exception e){
            loading = false;
            exceptionHandler.Invoke(e);
            return false;
        }
        return true;
    }

    public static T LoadAsset<T>(string path) where T:class{
        if(typeof(T) == typeof(Sprite))                     return ReturnResult<Sprite>(sprites, path) as T;
        else if(typeof(T) == typeof(AudioClip))             return ReturnResult<AudioClip>(sfx, path) as T;
        else if(typeof(T) == typeof(Locales))               return ReturnResult<Locales>(languages, path) as T;
        else if(typeof(T) == typeof(List<ColorRegion>))     return ReturnResult<List<ColorRegion>>(regions, path) as T;
        else if(typeof(T) == typeof(OtomatConfigs))     return ReturnResult<OtomatConfigs>(otomats, path) as T;
        else if(typeof(T) == typeof(StudentConfigs))     return ReturnResult<StudentConfigs>(students, path) as T;
        else if(typeof(T) == typeof(LocationProperties))     return ReturnResult<LocationProperties>(locations, path) as T;
        else return null;
    }

    public static T[] LoadAllAssets<T>(string path="", bool includeSubDirectories = false) where T:class{
        string[] targetKeys = new string[0];
        if(typeof(T) == typeof(Sprite)) targetKeys = sprites.Keys.ToArray();
        else if(typeof(T) == typeof(AudioClip)) targetKeys = sfx.Keys.ToArray();
        else if(typeof(T) == typeof(Locales)) targetKeys = languages.Keys.ToArray();
        else if(typeof(T) == typeof(List<ColorRegion>)) targetKeys = regions.Keys.ToArray();
        else if(typeof(T) == typeof(OtomatConfigs)) targetKeys = otomats.Keys.ToArray();
        else if(typeof(T) == typeof(StudentConfigs)) targetKeys = students.Keys.ToArray();
        else if(typeof(T) == typeof(LocationProperties)) targetKeys = locations.Keys.ToArray();
        string[] keys = path == "" ? targetKeys : targetKeys.Where((a) => (
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
