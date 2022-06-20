using UnityEngine;

public class BodyCreator : MonoBehaviour
{
    const string assetsPath = "CharacterAssets/";
    const string femalePath = "female";
    const string malePath = "male";
    const string unisexPath = "all";

    static readonly string[] CREATION_ORDER = {"body", "mouth", "nose", "eye", "eyebrow", "hair", "cloth"};
    [SerializeField] Color[] hairColors;

    GameObject CreateAsChild(Transform t){
        GameObject g = new GameObject();
        g.transform.SetParent(t);
        g.transform.localPosition = Vector3.zero;
        g.transform.localRotation = Quaternion.identity;
        g.transform.localScale = Vector3.one;
        return g;
    }

    void RemoveChildren(Transform t){
        for(int i = 0; i < t.childCount; i++)
            Destroy(t.GetChild(i).gameObject);
    }

    public void CreateBody() {
        RemoveChildren(transform);
        bool isFemale = Random.Range(1, 101) > 50;
        string genderPath = isFemale ? femalePath : malePath;

        for(int i = 0; i < CREATION_ORDER.Length; i++){
            string key = CREATION_ORDER[i];
            Sprite[] unisexSprites = Resources.LoadAll<Sprite>(assetsPath+key+"/"+unisexPath);
            Sprite[] genderSprites = Resources.LoadAll<Sprite>(assetsPath+key+"/"+genderPath);

            int unisexLength = unisexSprites == null ? 0 : unisexSprites.Length;
            int genderLength = genderSprites == null ? 0 : genderSprites.Length;

            int selectedIndex = Random.Range(0, unisexLength + genderLength);
            Sprite selectedSprite = selectedIndex >= unisexLength ? genderSprites[selectedIndex - unisexLength] : unisexSprites[selectedIndex];

            GameObject rendererObject = CreateAsChild(transform);
            SpriteRenderer thisRenderer = rendererObject.AddComponent<SpriteRenderer>();
            thisRenderer.sprite = selectedSprite;
            if (key == "hair"){
                thisRenderer.color = hairColors[Random.Range(0, hairColors.Length)];
            }
            rendererObject.transform.localPosition += new Vector3(0, 0, -i*0.01f);
        }
    }
}
