using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Configs
{
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

    StudentConfigs studentConfigs;
    ItemConfigs itemConfigs;

    static void CheckAndCreateInstances(){
        if(instance != null) return;
        instance = new Configs();
        if(instance.itemConfigs == null){
            instance.itemConfigs = JsonUtility.FromJson<ItemConfigs>(Resources.Load<TextAsset>(itemConfigsPath).ToString());
            instance.itemConfigs.CreateDictionaries();
        } if(instance.studentConfigs == null){
            instance.studentConfigs = JsonUtility.FromJson<StudentConfigs>(Resources.Load<TextAsset>(studentConfigsPath).ToString());
            instance.studentConfigs.CreateDictionaries();
        }
    }
}
