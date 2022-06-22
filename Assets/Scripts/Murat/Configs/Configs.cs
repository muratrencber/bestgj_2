using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}
