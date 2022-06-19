using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class PlayerStorage : DayEntity
{
    public bool gameEnded = false;
    public static PlayerStorage instance;
    class StorageItem{
        public Items item;
        public int count;
        public GameObject instantiatedObject;
    }
    [SerializeField] GameObject[] allItems;
    public Dictionary<Items, GameObject> prefabDict = new Dictionary<Items, GameObject>();

    List<StorageItem> items = new List<StorageItem>(); 
    int liraCount;
    [SerializeField] GameObject buttonContainer, textContainer;
    [SerializeField] TextMeshProUGUI text1, text2;
    [SerializeField] Transform spawnPosition;
    [SerializeField] Otomat otomat;
    StorageItem currentItem;
    int currentIndex = -1;

    void Awake(){
        gameEnded = false;
        instance = this;
        CreateDict();
        Add(Items.BIR_LIRA, 100);
    }

    void Start(){
        

        UIPrompt.Command cm = new UIPrompt.Command();
        cm.text = "Sınıf başkanlığı için yarışıyorsun";
        UIPrompt.Command cm2 = new UIPrompt.Command();
        cm2.text = "Sınıf arkadaşlarına güzel hediyeler alıp onların aklını çelmen lazım...";
        UIPrompt.Command cm3 = new UIPrompt.Command();
        cm3.text = "Ancak dikkat et, herkes her hediyeyi beğenmeyebilir. Verdiğin abur cuburların türüne göre tepkileri değişir.";
        UIPrompt.Command cm4 = new UIPrompt.Command();
        cm4.text = "Tüm eşyalarını harcadığında bir başkanlık seçimi yapılacak.";
        UIPrompt.Command cm5 = new UIPrompt.Command();
        cm5.text = "İyi şanslar.";

        cm.textDuration = 3;
        cm2.textDuration = 5;
        cm3.textDuration = 6;
        cm4.textDuration = 5;
        cm5.textDuration = 3;

        UIPrompt.AddCommand(cm);
        UIPrompt.AddCommand(cm2);
        UIPrompt.AddCommand(cm3);
        UIPrompt.AddCommand(cm4);
        UIPrompt.AddCommand(cm5);
        UIPrompt.StartEvaluating();
    }

    public override void OnDayEnded()
    {
        //throw new System.NotImplementedException();
    }
    public override void OnDayStarted()
    {
        //throw new System.NotImplementedException();
    }

    void CreateDict(){
        foreach(GameObject gj in allItems){
            prefabDict.Add(gj.GetComponent<Obje>().item, gj);
        }
    }

    public void Add(Items obj, int count = 1){
        int i = items.FindIndex((a) => (a.item == obj));
        if(i >= 0 && i < items.Count){
            items[i].count += count;
        }
        else {
            StorageItem it = new StorageItem();
            it.item = obj;
            it.count = count;
            i = items.Count;
            items.Add(it);
        }
        if(currentItem == null){
            SetItem(i);
        }
    }

    public void SetItemAway(int index){
        SetItem(currentIndex + index);
    }

    void SetItem(int index){
        if(items.Count <= 0){
            currentIndex = -1;
            if(currentItem != null && currentItem?.instantiatedObject){
                    DestroyImmediate(currentItem.instantiatedObject);
            }
            currentItem = null;
            return;
        }
        if(index < 0)
            index = items.Count - 1;
        currentIndex = index % items.Count;
        StorageItem it = items[currentIndex];
        if(currentItem?.instantiatedObject)
            DestroyImmediate(currentItem.instantiatedObject);
        it.instantiatedObject = Instantiate(prefabDict[it.item], spawnPosition);
        it.instantiatedObject.transform.localPosition = Vector3.zero;
        currentItem = it;
    }
    void SetItem(Items obj){
        if(items.Count <= 0)
            return;
        int i = items.FindIndex((a) => (a.item == obj));
        if (i >= 0)
            SetItem(i);
    }

    public void RemoveItem(Items obj, int count = 1){
        int i = items.FindIndex((a) => (a.item == obj));
        if(i >= 0 && i < items.Count){
            items[i].count -= count;
            if(items[i].count <= 0){
                if(items[i].instantiatedObject)
                    DestroyImmediate(items[i].instantiatedObject);
                items.RemoveAt(i);
                if(i == currentIndex){
                    currentItem = null;
                    currentIndex = -1;
                    SetItem(i - 1);
                }
            }
        }
    }

    void Update(){
        int totalCount = items.Count + (liraCount > 0 ? 1 : 0);
        if(totalCount == 0 && otomat.deposit < 2 && GameObject.FindObjectsOfType<InvObject>().Count((io)=>(io.enabled && io.gameObject.activeInHierarchy)) == 0 && !gameEnded){
            UIPrompt.Command cm = new UIPrompt.Command();
            cm.text = "Başkanlık seçimi yapıldı...";

            Student[] stds = GameObject.FindObjectsOfType<Student>();
            int voteCount = 0;
            foreach(Student std in stds){
                if(std.willVote())
                    voteCount++;
            }
            float ratio = (float) voteCount / (float)stds.Length;
            int otherCount = stds.Length - voteCount;
            ratio *= 100;
            UIPrompt.Command cm2 = new UIPrompt.Command();
            cm2.text = stds.Length+" kişiden "+voteCount+" kişi sana oy verdi.";
            UIPrompt.Command cm3 = new UIPrompt.Command();
            cm3.text = otherCount > voteCount ? "Kaybettin. Bir dahaki sefere artık..." : "Kazandın! Tebrikler.";

            cm.textDuration = 3;
            cm2.textDuration = 5;
            cm3.textDuration = 6;


            UIPrompt.AddCommand(cm);
            UIPrompt.AddCommand(cm2);
            UIPrompt.AddCommand(cm3);
            UIPrompt.StartEvaluating();

            gameEnded = true;
            return;
        }
        buttonContainer.SetActive(totalCount > 1);
        textContainer.SetActive(currentItem != null);
        if(currentItem != null){
            if(!currentItem.instantiatedObject){
                currentItem.instantiatedObject = Instantiate(prefabDict[currentItem.item], spawnPosition);
                currentItem.instantiatedObject.transform.localPosition = Vector3.zero;
            }
            string text = prefabDict[currentItem.item].GetComponent<Obje>().objeName + (currentItem.count > 1 ? "( x"+currentItem.count+")" : "");
            if(currentItem.item == Items.BIR_LIRA){
                text = currentItem.count +" Lira";
            }
            text1.text = text;
            text2.text = text;
        }
    }

}
