using UnityEngine;

public class ItemWorldObjectCreator
{
    const string spritesPath = "ItemSprites/";
    public static GameObject CreateItem(string key, Transform parent, float scaling = 1, float rotation = 0){
        ItemConfigs.ItemProperties ip = Configs.ItemConfigs.ItemDictionary[key];
        Sprite item_sprite = Resources.Load<Sprite>(spritesPath + ip.itemKey);

        GameObject g = new GameObject();
        SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
        sr.sprite = item_sprite;

        g.transform.SetParent(parent);
        g.transform.localPosition = Vector3.zero;
        g.transform.localScale = Vector3.one * scaling * ip.worldScaling;
        g.transform.localRotation = Quaternion.identity;

        g.transform.RotateAround(g.transform.position, Vector3.forward, rotation);
        g.transform.RotateAround(g.transform.position, Vector3.forward, ip.worldRotation);

        return g;
    }
}
