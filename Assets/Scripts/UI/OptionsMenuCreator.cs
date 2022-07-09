using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class OptionsMenuCreator
{
    public class Item<T>{
        public T itemClass;
        public GameObject itemInstance;
        public Transform container;

        public Item(T item, GameObject g) => Set(item, g, null);
        public Item(T item, GameObject g, Transform t) => Set(item, g, t);

        void Set(T item, GameObject g, Transform t){
            itemClass = item;
            itemInstance = g;
            container = t;
        }
    }
    public enum ItemType{
        LABEL,
        TOGGLE,
        DROPDOWN,
        SLIDER,
        BUTTON,
        ROW,
        COLUMN
    }
    struct ItemPaths{
        public Type objectType;
        public ItemType itemType;
        public string path;

        public ItemPaths(Type ot, string p, ItemType it){
            objectType = ot;
            path = p;
            itemType = it;
        }
    }

    static readonly ItemPaths[] paths = {   new ItemPaths(typeof(TextMeshProUGUI), "UI/Menu/Label", ItemType.LABEL),
                                            new ItemPaths(typeof(Toggle), "UI/Menu/Toggle", ItemType.TOGGLE),
                                            new ItemPaths(typeof(TMP_Dropdown), "UI/Menu/Dropdown", ItemType.DROPDOWN),
                                            new ItemPaths(typeof(Slider), "UI/Menu/Slider", ItemType.SLIDER),
                                            new ItemPaths(typeof(Button), "UI/Menu/Button", ItemType.BUTTON),
                                            new ItemPaths(typeof(Transform), "UI/Menu/Row", ItemType.ROW),
                                            new ItemPaths(typeof(Transform), "UI/Menu/Column", ItemType.COLUMN)
                                        };

    static GameObject GetPrefab(ItemType it){
        ItemPaths p = paths.Where((ip) => ip.itemType == it).First();
        return Resources.Load<GameObject>(p.path);
    }

    public static Item<T> CreateRow<T>(Transform container, ItemType t, LocalizedString labelKey, params LocalizedString[] additionalData){
        return CreateRowOrColumn<T>(container, t, labelKey, true, additionalData);
    }
    public static Item<T> CreateColumn<T>(Transform container, ItemType t, LocalizedString labelKey, params LocalizedString[] additionalData){
        return CreateRowOrColumn<T>(container, t, labelKey, false, additionalData);
    }

    static Item<T> CreateRowOrColumn<T>(Transform container, ItemType t, LocalizedString labelKey, bool isRow, params LocalizedString[] additionalData){
        Transform rowOrColumn = GameObject.Instantiate(GetPrefab(isRow ? ItemType.ROW : ItemType.COLUMN).gameObject, container).transform;
        if(labelKey != null) Create<TextMeshProUGUI>(rowOrColumn, ItemType.LABEL, labelKey);
        Item<T> item = Create<T>(rowOrColumn, t, additionalData);
        return new Item<T>(item.itemClass, item.itemInstance, rowOrColumn);
    }
    
    public static Item<T> Create<T>(Transform container, ItemType t, params LocalizedString[] additionalData){
        GameObject instance = GameObject.Instantiate(GetPrefab(t), container);
        T res = instance.GetComponentDownward<T>();
        switch(t){
            case ItemType.DROPDOWN:
                TMP_Dropdown resAsDD = res as TMP_Dropdown;
                resAsDD.ClearOptions();
                resAsDD.AddOptions(additionalData.Select((a) => a.GetValue()).ToList());
                break;

            case ItemType.BUTTON:
                AddLocalizedString(instance, additionalData[0]);
                break;

            case ItemType.LABEL:
                AddLocalizedString(instance, additionalData[0]);
                break;
        }
        return new Item<T>(res, instance);
    }

    public static void AddLocalizedString(GameObject g, LocalizedString key){
        TextMeshProUGUI txt = g.GetComponentDownward<TextMeshProUGUI>();
        if(key.isKey){
            LocalizedText localizedText = txt.GetComponent<LocalizedText>();
            if(localizedText == null) localizedText = txt.gameObject.AddComponent<LocalizedText>();
            localizedText.Set(key.keyOrText, key.isMod);
        } else {
            txt.text = key.keyOrText;
        }
    }
}
