using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerStorage : DayEntity
{
    struct StorageItem{
        public int count;
        public GameObject instantiatedObject;
    }
    [SerializeField] GameObject[] allItems;
    Dictionary<Items, GameObject> prefabDict = new Dictionary<Items, GameObject>();

    Dictionary<Items, StorageItem> items = new Dictionary<Items, StorageItem>(); 
    int liraCount;
    [SerializeField] GameObject buttonContainer, textContainer;
    [SerializeField] TextMeshProUGUI text1, text2;
    [SerializeField] Transform spawnPosition;
    Obje currentItem;
    int currentIndex = 0;

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
        if(items.ContainsKey(obj)){
            StorageItem it = items[obj];
            ++it.count;
            items[obj] = it;
        }
        else {
            StorageItem it;
            it.count = count;
            it.instantiatedObject = Instantiate(prefabDict[obj], spawnPosition);
            it.instantiatedObject.transform.localPosition = Vector3.zero;
            items.Add(obj, it);

        }
    }

    void SetItem(int index){
        
    }

    void Update(){
        int totalCount = items.Count + (liraCount > 0 ? 1 : 0);
        buttonContainer.SetActive(totalCount > 0);
        textContainer.SetActive(currentItem);
    }

}
