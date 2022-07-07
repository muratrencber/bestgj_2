using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class OptionsMenuCreator
{

    struct ItemPaths{
        public Type type;
        public string path;

        public ItemPaths(Type t, string p){
            type = t;
            path = p;
        }
    }

    static readonly ItemPaths[] paths = {   new ItemPaths(typeof(TextMeshProUGUI), "UI/Menu/Label"),
                                            new ItemPaths(typeof(Toggle), "UI/Menu/Toggle"),
                                            new ItemPaths(typeof(TMP_Dropdown), "UI/Menu/Dropdown"),
                                            new ItemPaths(typeof(Slider), "UI/Menu/Slider"),
                                            new ItemPaths(typeof(Button), "UI/Menu/Button"),
                                            new ItemPaths(typeof(Transform), "UI/Menu/Row")
                                        };

    static GameObject GetPrefab<T>(){
        ItemPaths p = paths.Where((ip) => ip.type == typeof(T)).First();
        return Resources.Load<GameObject>(p.path);
    }

    public static KeyValuePair<T, GameObject> Create<T>(Transform container, string labelKey, params string[] additionalData){
        IEnumerable<ItemPaths> found = paths.Where((p) => p.type == typeof(T));
        if(found == null || found.Count() <= 0) return new KeyValuePair<T, GameObject>(default(T), null);
        ItemPaths selected = found.First();

        Transform row = GameObject.Instantiate(GetPrefab<Transform>().gameObject, container).transform;
        
        if(labelKey != null && labelKey != ""){
            GameObject labelInstance = GameObject.Instantiate(GetPrefab<TextMeshProUGUI>(), row);
            LocalizedText localizedText = labelInstance.AddComponent<LocalizedText>();
            localizedText.Set(labelKey, false);
        }
        GameObject instance = GameObject.Instantiate(GetPrefab<T>(), row);
        T res = instance.GetComponentDownward<T>();
        if(typeof(T) == typeof(TMP_Dropdown)){
            TMP_Dropdown resAsDD = res as TMP_Dropdown;
            resAsDD.ClearOptions();
            resAsDD.AddOptions(additionalData.ToList());

        } else if(typeof(T) == typeof(Button)){
            Button resAsB = res as Button;
            LocalizedText localizedText = resAsB.gameObject.GetComponentDownward<TextMeshProUGUI>().gameObject.AddComponent<LocalizedText>();
            localizedText.Set(additionalData[0], false);
        }
        return new KeyValuePair<T, GameObject>(res, instance);
    }
}
