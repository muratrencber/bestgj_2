using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemConfigs
{
    const string resourcePath = "Configs/ItemConfigs.json";

    [System.Serializable]
    public struct ItemProperties{
        public string itemKey;
        public string itemName;
        public string[] tags;
        public int price;
        public float minAddition, maxAddition;
    }
    
    public Dictionary<string, ItemProperties> ItemDictionary {get{return itemDictionary;}}
    public ItemProperties[] ItemArray {get{return items;}}
    public float MaxAdditionMultiplier{get{return maxAdditionMultiplier;}}

    static ItemConfigs instance;
    [SerializeField] ItemProperties[] items;
    [SerializeField] float maxAdditionMultiplier;
    [SerializeField] int defaultMinAddition;

    Dictionary<string, ItemProperties> itemDictionary = new Dictionary<string, ItemProperties>();

    public void CreateItemDictionary(){
        itemDictionary.Clear();
        foreach(ItemProperties props in items){
            itemDictionary.Add(props.itemKey, props);
        }
    }
}
