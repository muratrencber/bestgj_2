using UnityEngine;
[System.Serializable]
public class Configurable
{
    public string ClassName{get{return className;}}
    
    [SerializeField] string className;
    public virtual void Init(){

    }
}
