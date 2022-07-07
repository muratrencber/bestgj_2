using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ResourceManager
{
    class DictionaryProperties{
        public string folderName;
        public string loadLangKey;
        public Action<string, IDictionary> loadingAction;
        public Action<string> nonDictLoadingAction;

        public DictionaryProperties(string fName, string lKey, Action<string, IDictionary> act){
            folderName = fName;
            loadLangKey = lKey;
            loadingAction = act;
        }

        public DictionaryProperties(string fName, string lKey, Action<string> act){
            folderName = fName;
            loadLangKey = lKey;
            nonDictLoadingAction = act;
        }
    }

    static bool initialized = false;
    static bool loading = false;

    static Dictionary<Type, IDictionary> dictionaries = new Dictionary<Type, IDictionary>();
    static Dictionary<Type, DictionaryProperties> dictProperties = new Dictionary<Type, DictionaryProperties>();

    static void Initialize(bool force = false){
        if(initialized && !force) return;

        ClearAll();

        CreateDictionaryOfType<Configurable>(
            new DictionaryProperties("configs", "loading_configs", ConfigruableLoader<Configurable>.LoadConfigs)
        );
        CreateDictionaryOfType<Sprite>(
            new DictionaryProperties("images", "loading_images", ImageLoader.LoadImages)
        );
        CreateDictionaryOfType<AudioClip>(
            new DictionaryProperties("sfx", "loading_sfx", AudioLoader.LoadSFX)
        );
        CreateDictionaryOfType<Locales>(
            new DictionaryProperties("locales", "loading_locales", LocalesLoader.LoadLocales)
        );
        CreateDictionaryOfType<List<ColorRegion>>(
            new DictionaryProperties("locations", "loading_regions", ColorRegionsLoader.LoadRegions)
        );
        CreateDictionaryOfType<OtomatConfigs>(
            new DictionaryProperties("automata", "loading_automata", ConfigruableLoader<OtomatConfigs>.LoadConfigs)
        );
        CreateDictionaryOfType<StudentConfigs>(
            new DictionaryProperties("students", "loading_custom_students", ConfigruableLoader<StudentConfigs>.LoadConfigs)
        );
        CreateDictionaryOfType<LocationProperties>(
            new DictionaryProperties("locations", "loading_locations", LocationsLoader.LoadLocations)
        );

        initialized = true;
    }

    static void CreateDictionaryOfType<T>(DictionaryProperties dctProps){
        Type t = typeof(T);
        if(!dictionaries.ContainsKey(t)){
            Dictionary<string, T> newDct = new Dictionary<string, T>();
            dictionaries.TryAdd(t, newDct);
            dictProperties.TryAdd(t, dctProps);
        }
    }

    static void ClearAll(){
        foreach(IDictionary dct in dictionaries.Values) dct.Clear();
        dictionaries.Clear();
        dictProperties.Clear();
    }
    
    public static IEnumerator LoadGame(string path, GameObject UIObject, TMPro.TextMeshProUGUI text, Action<Exception> exceptionHandler, Action onFinished){
        if(loading) yield break;
        loading = true;
        Initialize();
        float time = 0.5f;
        UIObject.SetActive(true);
        foreach(KeyValuePair<Type, IDictionary> typDct in dictionaries){
            DictionaryProperties dctProps = dictProperties[typDct.Key];
            IDictionary dct = typDct.Value;
            yield return SetPrompt(text, dctProps.loadLangKey, time);
            if(!TryLoad(dctProps.loadingAction, path + "/" + dctProps.folderName, dct, exceptionHandler)) yield break;
        }

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

    static bool TryLoad(Action<string, IDictionary> method, string path, IDictionary dct, Action<Exception> exceptionHandler){
        try{
            method.Invoke(path, dct);
        } catch(Exception e){
            loading = false;
            exceptionHandler.Invoke(e);
            return false;
        }
        return true;
    }

    static Dictionary<string, T> GetDict<T>(){
        if(dictionaries.TryGetValue(typeof(T), out IDictionary dct)){
            Dictionary<string, T> convertedDct = dct as Dictionary<string, T>;
            if(convertedDct != null) return convertedDct;
        }
        return null;
    }

    public static T LoadAsset<T>(string path) where T:class{
        Dictionary<string, T> dct = GetDict<T>();
        if(dct != null && dct.TryGetValue(path, out T val)) return val;
        return null;
    }


    public static T[] LoadAllAssets<T>(string path="", bool includeSubDirectories = false) where T:class{
        string[] targetKeys = new string[0];

        Dictionary<string,T> dct = GetDict<T>();
        if(dct != null) targetKeys = dct.Keys.ToArray();

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

    public static T[] GetAll<T>() where T:class => LoadAllAssets<T>("",true);
}
