using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Student : DayEntity
{
    private Dictionary<string, float> points;
    List<string> likes = new List<string>();
    List<string> dislikes = new List<string>();
    public float positivity;
    [SerializeField] BodyCreator bodyCreator;
    [SerializeField] TextMeshProUGUI t1, t2;

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
        bodyCreator.CreateBody();
        ClearText();
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
        float total = 0;
        Obje o = gameObject.GetComponent<Obje>();
        ItemConfigs.ItemProperties ip = Configs.ItemConfigs.ItemDictionary[o.key];
        foreach(string t in ip.tags)
        {
            total += points[t];
        }
        total += points[ip.itemKey];
        total /= (float)(ip.tags.Length + 1);
        positivity += total;
        if(total > 0){
            bool likesItem = likes.Contains(ip.itemKey);
            List<string> intersection = likes.Intersect(ip.tags).ToList();
            //0 -> generic like, 1 -> specific type like, 2 -> generic item like
            int type = (likesItem && Random.Range(1,101) > 80) ? 2 : intersection.Count > 0 ? 1 : 0;
            List<StudentConfigs.Dialogue> dialogueList;
            string text = "...";
            if(type == 0){
                dialogueList = Configs.StudentConfigs.GetDialogues(StudentConfigs.Dialogue.Type.LIKE_ITEM);
            } else if(type == 1){
                string selected = intersection[Random.Range(0, intersection.Count)];
                dialogueList = Configs.StudentConfigs.GetDialogues(StudentConfigs.Dialogue.Type.LIKE_TYPE, selected);
            } else {
                dialogueList = Configs.StudentConfigs.GetDialogues(StudentConfigs.Dialogue.Type.LIKE_ITEM, "", true);
            }
            if(dialogueList.Count > 0)
                text = dialogueList[Random.Range(0, dialogueList.Count)].Evaluate(ip.itemName);
            SetText(text);
        } else{
            List<StudentConfigs.Dialogue> dialogueList = Configs.StudentConfigs.GetDialogues(StudentConfigs.Dialogue.Type.DISLIKE);
            SetText(dialogueList[Random.Range(0, dialogueList.Count)].Evaluate(ip.itemKey));
        }
    }

    public bool willVote(){
        return positivity > Configs.StudentConfigs.WinTolerance;
    }

    void SetLikes() {
        points = new Dictionary<string, float>();
        float point;
        StudentConfigs scfgs = Configs.StudentConfigs;
        ItemConfigs icfgs = Configs.ItemConfigs;
        foreach (ItemConfigs.TagProperties tp in icfgs.TagArray)
        {
            point = Random.Range(scfgs.MinLike, scfgs.MaxLike);
            if(point >= 0)
                likes.Add(tp.key);
            else
                dislikes.Add(tp.key);
            points.Add(tp.key, point);
        }
        foreach (ItemConfigs.ItemProperties ip in icfgs.ItemArray)
        {
            point = Random.Range(icfgs.DefaultMinAddition, ip.price * icfgs.MaxAdditionMultiplier);
            if(point > 0)
                likes.Add(ip.itemKey);
            else
                dislikes.Add(ip.itemKey);
            points.Add(ip.itemKey, point);
        }
    }

    

}
