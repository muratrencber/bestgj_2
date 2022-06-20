using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigTesting : MonoBehaviour
{
    void Start(){
        Debug.Log(Configs.ItemConfigs.ItemDictionary["brobis"].itemName);
    }
}
