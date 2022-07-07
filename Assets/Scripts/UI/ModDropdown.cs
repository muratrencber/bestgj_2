using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ModDropdown : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;

    void OnEnable(){
        SetModPaths();
    }

    void SetModPaths(){
        dropdown.ClearOptions();
        
        GameLoader.Properties[] props = GameLoader.LoadGames();
        List<string> options = GameLoader.LoadGames().Select((p) => p.GamePath.Split("games\\")[1]).ToList();
        
        dropdown.AddOptions(options);
    }
}
