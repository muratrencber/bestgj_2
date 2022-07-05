using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OtomatConfigs: Configurable
{
    public enum Type{
        FOOD,
        DRINK
    }
    public Type OtomatType {get{return type;}}
    public StateProperties[] Properties {get{return properties;}}
    public string[] ItemKeys {get{return itemKeys;}}
    public int StartNumber {get{return startNumber;}}
    public int SizeX {get{return sizeX;}}
    public int SizeY {get{return sizeY;}}
    public float DurationeBetweenCoins {get{return durationBetweenCoins;}}

    [System.Serializable]
    public class StateProperties{
        public string prompt;
        public float duration = 0;
        public string stateName;
        public Otomat.State state;
        public string startSound = "";
        public string loopSound = "";
        public string endSound = "";

        public void SetState(){
            if(System.Enum.TryParse<Otomat.State>(stateName, true, out Otomat.State result)) state = result;
            else state = Otomat.State.NO_MONEY;
        }
    }

    [System.Serializable]
    public class SoundKeys{
        public string key;
        public string value;
    }
    [SerializeField] string key;
    [SerializeField] string typeName;
    [SerializeField] StateProperties[] properties;
    [SerializeField] Dictionary<Otomat.State, StateProperties> propertiesDictionary = new Dictionary<Otomat.State, StateProperties>();
    [SerializeField] string[] itemKeys;
    [SerializeField] int startNumber = 10;
    [SerializeField] int sizeX = 1;
    [SerializeField] int sizeY = 1;
    [SerializeField] float durationBetweenCoins;
    [SerializeField] SoundKeys[] sfx;
    Type type;
    public void CreateDictionaries(){}

    public override void Init(){
        if(System.Enum.TryParse<Type>(typeName, true, out Type t)) type = t;
        else type = Type.FOOD;

        foreach(StateProperties sp in properties){
            sp.SetState();
            propertiesDictionary.TryAdd(sp.state, sp);
        }
    }
}
