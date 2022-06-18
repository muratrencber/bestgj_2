using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : DayEntity
{
    //set paths
    const string assetsPath = "CharacterAssets/";
    const string femalePath = "female";
    const string malePath = "male";
    const string unisexPath = "all";

    static readonly string[] CREATION_ORDER = {"body", "mouth", "nose", "eye", "eyebrow", "hair", "cloth"};
    

    private IDictionary<Types, int> puanlar;
    private IDictionary<Items, int> puanlarItem;
    [SerializeField]
    private int min, max, minItems, maxItems;
    //[SerializeField]
    //private int credit;
    [SerializeField]
    private GameObject gameObject1;
    int positivity;
    //public Sprite[] body;
    [SerializeField] Color[] hairColors;
    Transform spritesContainer;
    bool isFemale;

    GameObject CreateAsChild(Transform parent){
        GameObject obj = new GameObject();
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        return obj;
    }

    void CreateBody() {
        isFemale = Random.Range(1, 101) > 50;
        string genderPath = isFemale ? femalePath : malePath;

        //set containe
        spritesContainer = CreateAsChild(transform).transform;

        for(int i = 0; i < CREATION_ORDER.Length; i++){
            string key = CREATION_ORDER[i];
            Sprite[] unisexSprites = Resources.LoadAll<Sprite>(assetsPath+key+"/"+unisexPath);
            Sprite[] genderSprites = Resources.LoadAll<Sprite>(assetsPath+key+"/"+genderPath);

            int unisexLength = unisexSprites == null ? 0 : unisexSprites.Length;
            int genderLength = genderSprites == null ? 0 : genderSprites.Length;

            int selectedIndex = Random.Range(0, unisexLength + genderLength);
            Sprite selectedSprite = selectedIndex >= unisexLength ? genderSprites[selectedIndex - unisexLength] : unisexSprites[selectedIndex];

            GameObject rendererObject = CreateAsChild(spritesContainer);
            SpriteRenderer thisRenderer = rendererObject.AddComponent<SpriteRenderer>();
            thisRenderer.sprite = selectedSprite;
            if (key == "hair"){
                thisRenderer.color = hairColors[Random.Range(0, hairColors.Length)];
            }
            rendererObject.transform.localPosition += new Vector3(0, 0, -i*0.01f);
        }
    }

    void SetLikes() {
        puanlar = new Dictionary<Types, int>();
        puanlarItem = new Dictionary<Items, int>();
        int point;
        foreach (Types type in System.Enum.GetValues(typeof(Types)))
        {
            point = Random.Range(min, max);
            puanlar.Add(type, point);
        }
        foreach (Items item in System.Enum.GetValues(typeof(Items)))
        {
            point = Random.Range(minItems, maxItems);
            puanlarItem.Add(item, point);
        }
    }

    void OnReceivedItem() {


    }

    void InspectItem() {


    }

    void MakeMark() {


    }

    void Vote() {

        
    }
    
    public override void OnDayEnded()
    {
        throw new System.NotImplementedException();
    }
    public override void OnDayStarted()
    {
        throw new System.NotImplementedException();
    }


        
    //private bool isInited = true;
    // Start is called before the first frame update
    void Start()
    {

        //credit = 0;
        //Init();
        CreateBody();
        //GetPoint(gameObject1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetPoint(GameObject gameObject)
    {
        int total = 0;
        Types[] typeArr = gameObject.GetComponent<Obje>().types;
        Items item = gameObject.GetComponent<Obje>().item;
        foreach(Types t in typeArr)
        {
            total += puanlar[t];
        }
        total += puanlarItem[item];
        float ratio = (float) total / (typeArr.Length * max + maxItems);
        Debug.Log(ratio);
        Debug.Log(total);
        
    }

    /*public void Init()
    {
        puanlar = new Dictionary<Types, int>();
        puanlarItem = new Dictionary<Items, int>();
        int point;
        foreach (Types type in System.Enum.GetValues(typeof(Types)))
        {
            point = Random.Range(min, max);
            puanlar.Add(type, point);
        }
        foreach (Items item in System.Enum.GetValues(typeof(Items)))
        {
            point = Random.Range(minItems, maxItems);
            puanlarItem.Add(item, point);
        }


    }*/

}
