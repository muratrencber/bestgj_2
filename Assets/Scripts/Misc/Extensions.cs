using UnityEngine;

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
}
