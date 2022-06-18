using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waifu : MonoBehaviour
{
    private IDictionary<Types, int> puanlar;
    private IDictionary<Items, int> puanlarItem;
    [SerializeField]
    private int min, max, minItems, maxItems;
    [SerializeField]
    private int credit;
    [SerializeField]
    private GameObject gameObject1;
    //private bool isInited = true;
    // Start is called before the first frame update
    void Start()
    {

        credit = 0;
        Init();
        GetPoint(gameObject1);
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

    public void Init()
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


    }
}
