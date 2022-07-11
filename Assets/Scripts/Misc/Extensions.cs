using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public static class Extensions
{
    public static void RedrawCSFUpwards(this RectTransform rt, bool forceAll = false){
        RectTransform current = rt;
        while(current != null){
            LayoutRebuilder.ForceRebuildLayoutImmediate(current);
            current = current.parent != null ? current.parent.GetComponent<RectTransform>() : null;
        }
        Canvas.ForceUpdateCanvases();
    }

    public static void SetStyle(this Button b, Color background, Color text){
        Color.RGBToHSV(background, out float h, out float s, out float v);
        Color variant1 = Color.HSVToRGB(h, s, v - 0.04f);
        Color variant2 = Color.HSVToRGB(h, s, v - 0.22f);
        Color variant3 = variant2;
        variant1.a = 1;
        variant2.a = 1;
        variant3.a = 0.5f;

        ColorBlock newBlock = b.colors;
        newBlock.normalColor = background;
        newBlock.highlightedColor = variant1;
        newBlock.pressedColor = variant2;
        newBlock.selectedColor = variant1;
        newBlock.disabledColor = variant3;

        b.colors = newBlock;

        TMPro.TextMeshProUGUI txt = b.gameObject.GetComponentDownward<TMPro.TextMeshProUGUI>();
        if(txt != null) txt.color = text;
    }
    
    public static void Toggle(this GameObject g){
        g.SetActive(!g.activeSelf);
    }

    public static void DestroyChildren(this Transform t){
        for(int i = 0; i < t.childCount; i++)
            GameObject.Destroy(t.GetChild(i).gameObject);
    }

    public static GameObject CreateEmptyChild(this Transform t){
        GameObject newObject = new GameObject();
        newObject.transform.SetParent(t);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localScale = Vector3.one;
        newObject.transform.localRotation = Quaternion.identity;
        return newObject;
    }

    public static T GetComponentDownward<T>(this GameObject g){
        T result = g.GetComponent<T>();
        if(result != null) return result;
        result = g.GetComponentInChildren<T>();
        if (result != null) return result;
        return default(T);
    }

    public static T GetComponentUpward<T>(this GameObject g){
        T result = g.GetComponent<T>();
        if(result != null) return result;
        result = g.GetComponentInParent<T>();
        if (result != null) return result;
        return default(T);
    }

    public static T GetComponentFull<T>(this GameObject g, bool upwardFirst = false){
        T res = upwardFirst ? g.GetComponentUpward<T>() : g.GetComponentDownward<T>();
        if(res != null) return res;
        return upwardFirst ? g.GetComponentInChildren<T>() : g.GetComponentInParent<T>();
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
