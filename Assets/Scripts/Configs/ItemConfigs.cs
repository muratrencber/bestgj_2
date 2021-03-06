using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemConfigs: Configurable
{
    const string resourcePath = "Configs/ItemConfigs.json";

    [System.Serializable]
    public class ItemProperties{
        public enum Type{
            FOOD,
            DRINK
        }
        public string itemKey;
        public string itemName;
        public string typeName = "FOOD";
        public List<string> tags = new List<string>();

        public int price;
        public float minAddition, maxAddition;
        public float worldRotation = 0;
        public float worldScaling = 1;
        public Type type;

        public void Init(){
            if(System.Enum.TryParse<Type>(typeName, true, out Type result)) type = result;
            else type = Type.FOOD;
        }
    }

    [System.Serializable]
    public class TagProperties{
        public string key;
        public string name;
        public int generality = 1;
    }
    
    public Dictionary<string, ItemProperties> ItemDictionary {get{return itemDictionary;}}
    public Dictionary<string, TagProperties> TagDictionary {get{return tagDictionary;}}
    public ItemProperties[] ItemArray {get{return items.ToArray();}}
    public TagProperties[] TagArray {get{return tags.ToArray();}}
    public float MaxAdditionMultiplier{get{return maxAdditionMultiplier;}}
    public float DefaultMinAddition{get{return defaultMinAddition;}}

    public List<int> testIntegers = new List<int>();
    static ItemConfigs instance;
    [SerializeField] List<ItemProperties> items = new List<ItemProperties>();
    [SerializeField] List<TagProperties> tags = new List<TagProperties>();
    [SerializeField] float maxAdditionMultiplier;
    [SerializeField] float defaultMinAddition;

    Dictionary<string, ItemProperties> itemDictionary = new Dictionary<string, ItemProperties>();
    Dictionary<string, TagProperties> tagDictionary = new Dictionary<string, TagProperties>();

    public void CreateDictionaries(){
        itemDictionary.Clear();
        foreach(ItemProperties props in items){
            props.Init();
            itemDictionary.Add(props.itemKey, props);
        }
        tagDictionary.Clear();
        foreach(TagProperties props in tags){
            tagDictionary.Add(props.key, props);
        }
    }
}
