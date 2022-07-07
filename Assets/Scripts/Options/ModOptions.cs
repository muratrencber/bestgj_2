using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class ModOptions : MonoBehaviour
{
    [SerializeField] Transform container;

    void Start(){
        CreateMenu();
    }
    void CreateMenu(){
        container.DestroyChildren();

        KeyValuePair<TMP_Dropdown, GameObject> modSelect = OptionsMenuCreator.Create<TMP_Dropdown>(container, "options_mod_select", GameLoader.LoadGames().Select((p) => p.GamePath.Split("games\\")[1]).ToArray());
        KeyValuePair<Toggle, GameObject> compress = OptionsMenuCreator.Create<Toggle>(container, "options_mod_compress");
        KeyValuePair<Toggle, GameObject> folder = OptionsMenuCreator.Create<Toggle>(container, "options_mod_folder");
        KeyValuePair<Toggle, GameObject> keep = OptionsMenuCreator.Create<Toggle>(container, "options_mod_keep");
        KeyValuePair<Button, GameObject> optimize = OptionsMenuCreator.Create<Button>(container, "","options_mod_optimize");
    }
}
