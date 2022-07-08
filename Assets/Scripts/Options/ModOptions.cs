using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class ModOptions : MonoBehaviour
{
    [SerializeField] Transform container;

    List<ToggleWatcher> watchers = new List<ToggleWatcher>();

    void Start(){
        CreateMenu();
    }
    void CreateMenu(){
        container.DestroyChildren();
        GameLoader.Properties[] props = GameLoader.LoadGames();
        string[] options = props.Select((p) => p.GamePath.Split("games/")[1]).ToArray();
        KeyValuePair<TMP_Dropdown, GameObject> modSelect = OptionsMenuCreator.Create<TMP_Dropdown>(container, "options_mod_select", options);
        KeyValuePair<Toggle, GameObject> compress = OptionsMenuCreator.Create<Toggle>(container, "options_mod_compress");
        KeyValuePair<Toggle, GameObject> folder = OptionsMenuCreator.Create<Toggle>(container, "options_mod_folder");
        KeyValuePair<Button, GameObject> optimize = OptionsMenuCreator.Create<Button>(container, "","options_mod_optimize");
    
        optimize.Key.onClick.AddListener(() => OptimizeMod(options[modSelect.Key.value], props[modSelect.Key.value], folder.Key.isOn, compress.Key.isOn));
    }

    void CreateToggleWatcher(bool enableWhenToggled, GameObject target, params Toggle[] toggles){
        if(toggles == null || toggles.Length == 0) return;
        watchers.Add(new ToggleWatcher(toggles.ToList(), enableWhenToggled, target));
    }

    void Update(){
        watchers.ForEach((w) => w.Update());
    }

    void OptimizeMod(string modFolder, GameLoader.Properties props, bool sameFolder, bool compress){
        string fullPath = string.Format("{0}/{1}/{2}",Application.streamingAssetsPath, GameLoader.path, modFolder);
        string targetPath = string.Format("{0}/{1}/{2}",Application.streamingAssetsPath, GameLoader.path, modFolder+"_optimized");
        int index = 1;
        while(System.IO.Directory.Exists(targetPath)){
            targetPath = string.Format("{0}/{1}/{2}",Application.streamingAssetsPath, GameLoader.path, modFolder+"_optimized_"+index);
            ++index;
        }
        string[] folders = ResourceManager.GetFolders();
        foreach(string folderName in folders){
            string fullSourceFolderPath = fullPath+"/"+folderName;
            string fullTargetFolderPath = targetPath+"/"+folderName;
            if(folderName == "images"){
                string[] onlyExts = {".json"};
                CloneDirectory.Start(fullSourceFolderPath, fullTargetFolderPath, onlyExts);
                ImageLoader.OptimizeImages(fullSourceFolderPath, fullTargetFolderPath, compress);
            } else if(folderName == "locations"){
                string[] onlyExts = {".loc"};
                CloneDirectory.Start(fullSourceFolderPath, fullTargetFolderPath, onlyExts);
                ColorRegionsLoader.OptimizeRegions(fullSourceFolderPath, fullTargetFolderPath);
            } else {
                CloneDirectory.Start(fullSourceFolderPath, fullTargetFolderPath, null, ".meta");
            }
        }
        
        props.SetOptimized();
        System.IO.File.WriteAllText(targetPath+"/properties.json", JsonUtility.ToJson(props, true));
    }
}
