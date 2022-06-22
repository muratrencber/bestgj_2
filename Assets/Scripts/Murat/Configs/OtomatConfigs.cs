using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OtomatConfigs
{
    public StateProperties[] Properties {get{return properties;}}
    public string[] ItemKeys {get{return itemKeys;}}
    public int StartNumber {get{return startNumber;}}
    public int SizeX {get{return sizeX;}}
    public int SizeY {get{return sizeY;}}
    public float DurationeBetweenCoins {get{return durationeBetweenCoins;}}

    [System.Serializable]
    public class StateProperties{
        public string prompt;
        public float duration = 0;
    }

    [SerializeField] StateProperties[] properties;
    [SerializeField] string[] itemKeys;
    [SerializeField] int startNumber;
    [SerializeField] int sizeX, sizeY;
    [SerializeField] float durationeBetweenCoins;
    public void CreateDictionaries(){}
}
