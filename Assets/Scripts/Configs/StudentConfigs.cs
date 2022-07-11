
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class StudentConfigs: Configurable
{
    public float MinLike {get{return minLike;}}
    public float MaxLike {get{return maxLike;}}
    public float WinTolerance {get{return winTolerance;}}

    [System.Serializable]
    public class Dialogue {
        public enum Type{
            LIKE_TYPE,
            LIKE_ITEM,
            DISLIKE
        }
        public string mainPart;
        public string secondaryPart = "";
        public string target = "";
        public Type t;
        public string sound;

        public string Evaluate(string input){
            if(secondaryPart == "")
                return mainPart;
            return mainPart + input + secondaryPart;
        }
    }

    [System.Serializable]
    public class Like{
        public string key;
        public float value;
    }
    [SerializeField] string color;
    [SerializeField] List<string> bodySources = new List<string>();
    [SerializeField] List<Dialogue> dialogues = new List<Dialogue>();
    [SerializeField] float minLike, maxLike;
    [SerializeField] float winTolerance;
    [SerializeField] List<Like> overrideLikes = new List<Like>();

    Dictionary<Dialogue.Type, List<Dialogue>> dialogueDictionary = new Dictionary<Dialogue.Type, List<Dialogue>>();

    public List<Dialogue> GetDialogues(Dialogue.Type targetType, string targetKey = "", bool specific = false){
        IEnumerable<Dialogue> ls = dialogueDictionary[targetType].Where((a) => ((a.secondaryPart != "") == specific));
        if(targetKey != "")
            return ls.Where((a)=>(a.target == targetKey)).ToList();
        return ls.ToList();
    }

    public void CreateDictionaries(){
        dialogueDictionary.Clear();
        foreach(Dialogue d in dialogues){
            if(!dialogueDictionary.ContainsKey(d.t))
                dialogueDictionary[d.t] = new List<Dialogue>();
            dialogueDictionary[d.t].Add(d);
        }
    }
}
