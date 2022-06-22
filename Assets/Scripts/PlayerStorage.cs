using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class PlayerStorage : DayEntity
{
    public bool HasMoney {get{return deposit > 0;}}
    public int Deposit {get{return deposit;}}
    public static PlayerStorage instance;
    class StorageItem{
        public string itemKey;
        public int count;
    }

    Dictionary<StorageItem, int> items = new Dictionary<StorageItem, int>(); 
    int deposit;

    void Awake(){
        instance = this;
        AddCoin(20);
    }

    void GameStart(){
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
    public void AddCoin(int count = 1){
        deposit += count;
    }
    public void RemoveCoin(int count = 1){
        deposit -= count;
    }
    public void AddItem(Items obj, int count = 1){
    }
    public void RemoveItem(Items obj, int count = 1){
    }

    void Update(){
        if(false){
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
            return;
        }
    }

}
