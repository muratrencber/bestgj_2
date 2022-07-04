using UnityEngine;
using System.Collections.Generic;

public static class Extensions
{
    public static void DestroyChildren(this Transform t){
        for(int i = 0; i < t.childCount; i++)
            GameObject.Destroy(t.gameObject);
    }

    public static GameObject CreateEmptyChild(this Transform t){
        GameObject newObject = new GameObject();
        newObject.transform.SetParent(t);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localScale = Vector3.one;
        newObject.transform.localRotation = Quaternion.identity;
        return newObject;
    }

    public static bool TryAddValue<K,V>(this Dictionary<K,V> dct, K key, V value, bool overwrite = false){
        if(dct.ContainsKey(key)){
            if(overwrite) return false;
            dct[key] = value;
            return true;
        }
        dct.Add(key, value);
        return true;
    }
}
