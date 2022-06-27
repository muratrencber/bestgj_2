using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


public class Configs
{
    const string otomatConfigsPath = "Configs/Otomat";
    const string studentConfigsPath = "Configs/Students";
    const string itemConfigsPath = "Configs/Items";
    static Configs instance;

    public static StudentConfigs StudentConfigs {
        get{
            CheckAndCreateInstances();
            return instance.studentConfigs;
        }
    }
    public static ItemConfigs ItemConfigs {
        get{
            CheckAndCreateInstances();
            return instance.itemConfigs;
        }
    }
    public static OtomatConfigs OtomatConfigs {
        get{
            CheckAndCreateInstances();
            return instance.otomatConfigs;
        }
    }

    StudentConfigs studentConfigs;
    ItemConfigs itemConfigs;
    OtomatConfigs otomatConfigs;

    static Dictionary<System.Type, Configurable> configurables = new Dictionary<System.Type, Configurable>();

    static void CheckAndCreateInstances(){
        if(instance != null) return;
        instance = new Configs();
        if(instance.itemConfigs == null){
            instance.itemConfigs = JsonUtility.FromJson<ItemConfigs>(Resources.Load<TextAsset>(itemConfigsPath).ToString());
            instance.itemConfigs.CreateDictionaries();
        } if(instance.studentConfigs == null){
            instance.studentConfigs = JsonUtility.FromJson<StudentConfigs>(Resources.Load<TextAsset>(studentConfigsPath).ToString());
            instance.studentConfigs.CreateDictionaries();
        }if(instance.otomatConfigs == null){
            instance.otomatConfigs = JsonUtility.FromJson<OtomatConfigs>(Resources.Load<TextAsset>(otomatConfigsPath).ToString());
            instance.otomatConfigs.CreateDictionaries();
        }
    }

    public static void LoadConfigurables(string path){
        configurables.Clear();
        string[] files = Directory.GetFiles(path).Where((a) => (Path.GetExtension(a) == ".json")).ToArray();
        foreach(string s in files){
            string content = File.ReadAllText(s);
            Configurable tempC = JsonUtility.FromJson<Configurable>(content);
            System.Type targetConfigurable = System.Type.GetType(tempC.ClassName);
            if(targetConfigurable == null) continue;
            else if(configurables.ContainsKey(targetConfigurable)) continue;
            Configurable targetObject = JsonUtility.FromJson(content, targetConfigurable) as Configurable;
            configurables.Add(targetConfigurable, targetObject);
        }
    }

    public static T GetConfigsOf<T>() where T:Configurable{
        System.Type targetType = typeof(T);
        if(configurables.TryGetValue(targetType, out Configurable val))
            return val as T;
        return null;
    }
}
