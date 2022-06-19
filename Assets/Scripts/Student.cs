using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Student : DayEntity
{
    struct PartialD
    {
        string first;
        string last;

        public PartialD(string one, string two){
            first = one;
            last = two;
        }

        public string Evaluate(string inObj){
            return first +" "+inObj+" "+last;
        }
    }

    //set paths
    const string assetsPath = "CharacterAssets/";
    const string femalePath = "female";
    const string malePath = "male";
    const string unisexPath = "all";

    static readonly string[] CREATION_ORDER = {"body", "mouth", "nose", "eye", "eyebrow", "hair", "cloth"};
    

    private Dictionary<Types, int> puanlar;
    private Dictionary<Items, int> puanlarItem;
    [SerializeField]
    private int min, max, minItems, maxItems;
    [SerializeField] float maxLikeRatio, ratioMult, threshold;
    //[SerializeField]
    //private int credit;
    [SerializeField]
    private GameObject gameObject1;
    public float positivity;
    //public Sprite[] body;
    [SerializeField] Color[] hairColors;
    Transform spritesContainer;
    bool isFemale;
    [SerializeField] TextMeshProUGUI t1, t2;

    Dictionary<Types, string> typeLikeDialogues = new Dictionary<Types, string>();
    List<string> dislikeDialogues = new List<string>();
    List<PartialD> genericLikeDialogues = new List<PartialD>();

    GameObject CreateAsChild(Transform parent){
        GameObject obj = new GameObject();
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        return obj;
    }

    float timer;

    void Update(){
        if(timer > 0){
            timer -= Time.deltaTime;
            if(timer <= 0)
                ClearText();
        }
    }
        
    void Start()
    {
        ClearText();
        SetDialogues();
        CreateBody();
        SetLikes();
    }
    
    public override void OnDayEnded()
    {
        ClearText();
    }
    public override void OnDayStarted()
    {
        ClearText();
    }

    void ClearText()=>SetText("");

    void SetText(string t, float duration = 4){
        t1.text = t;
        t2.text = t;
        timer = duration;
    }



    public void OnReceivedItem(GameObject gameObject)
    {
        int total = 0;
        Obje o = gameObject.GetComponent<Obje>();
        Types[] typeArr = o.types;
        Items item = o.item;
        foreach(Types t in typeArr)
        {
            total += puanlar[t];
        }
        total += puanlarItem[item];
        float ratio = (float) total / (typeArr.Length * max + maxItems);
        ratio *= ratioMult;
        positivity += ratio;
        if(ratio > 0){
            bool specific = Random.Range(1, 101) > 80;
            string text = "TXT";
            if(specific)
                text = genericLikeDialogues[Random.Range(0,genericLikeDialogues.Count)].Evaluate(o.objeName);
            else
                text = typeLikeDialogues[o.types[Random.Range(0, o.types.Length)]];
            SetText(text);
        } else{
            SetText(dislikeDialogues[Random.Range(0, dislikeDialogues.Count)]);
        }
    }

    public bool willVote(){
        return positivity > threshold;
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

    void SetDialogues(){
        typeLikeDialogues.Add(Types.tatli, "Canım tam da TATLI bir şeyler istemişti.");
        typeLikeDialogues.Add(Types.tuzlu, "Bunu normalde pek dillendirmem fakat... TUZLU şeyleri çok severim.");
        typeLikeDialogues.Add(Types.eksi, "Yüz EKŞİTEN türdendi... Yani mükemmel!");
        typeLikeDialogues.Add(Types.doyurucu, "Böyle DOYURUCU bir şey uzun zamandır yememiştim.");
        typeLikeDialogues.Add(Types.yiyecek, "Hmm... YİYECEK güzelmiş.");
        typeLikeDialogues.Add(Types.aci, "ACIYMIŞ... Leziz!");
        typeLikeDialogues.Add(Types.aburCubur, "Annem ABUR CUBUR yememi yasaklıyor ama... Sanırım bir seferden bir şey olmaz UwU");
        typeLikeDialogues.Add(Types.kremali, "İçindeki KREMA en sevdiğimden!");
        typeLikeDialogues.Add(Types.kitir, "KITIR KITIR en sevdiğim!");
        typeLikeDialogues.Add(Types.baharatli, "Bana mı? Hem de bol BAHARATLI!");
        typeLikeDialogues.Add(Types.biskuvi, "BİSKÜVİ severim!");
        typeLikeDialogues.Add(Types.cikolatali, "ÇİKOLATA'ya asla hayır demem!");
        typeLikeDialogues.Add(Types.icecek, "Tam da sussuzluğumu giderecek bir İÇECEK!");
        typeLikeDialogues.Add(Types.sicak, "SICAK bir şeyler almak iyi geldi. Teşekkür ederim.");
        typeLikeDialogues.Add(Types.soguk, "İçim SERİNLEDİ, sağ ol.");

        dislikeDialogues.Add("Bu da ne, bundan nefret ettiğimi bilmiyor musun!?");
        dislikeDialogues.Add("Ehh.. peki.");
        dislikeDialogues.Add("Bu iğrenç şeyi hediye ederek oy alabileceğini sanıyorsan yanılıyorsun.");
        dislikeDialogues.Add("Hayır... istemiyorum.");

        genericLikeDialogues.Add(new PartialD("Bu","var ya, favorimdir!"));
        genericLikeDialogues.Add(new PartialD("","sevdiğimi bilmene şaşırdım!"));
        genericLikeDialogues.Add(new PartialD("Yaşasın!","be!"));
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

}
