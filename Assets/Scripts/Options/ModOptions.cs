using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class ModOptions : MonoBehaviour
{
    [SerializeField] Transform container;

    List<Checker> checkers = new List<Checker>();

    void Start(){
        CreateMenu();
    }
    void CreateMenu(){
        checkers.Clear();
        container.DestroyChildren();
        GameLoader.Properties[] props = GameLoader.LoadGames();
        string[] options = props.Select((p) => p.GamePath.Split("games/")[1]).ToArray();

        OptionsMenuCreator.Item<TMP_Dropdown> modSelect = OptionsMenuCreator.CreateRow<TMP_Dropdown> (
            container, OptionsMenuCreator.ItemType.DROPDOWN,
            new LocalizedString("options_mod_select", false), options.Select((s) => new LocalizedString(s)).ToArray()
        ); OptionsMenuCreator.Item<Toggle> compress = OptionsMenuCreator.CreateRow<Toggle> (
            container, OptionsMenuCreator.ItemType.TOGGLE, 
            new LocalizedString("options_mod_compress", false)
        ); OptionsMenuCreator.Item<Toggle> folder = OptionsMenuCreator.CreateRow<Toggle> (
            container, OptionsMenuCreator.ItemType.TOGGLE, 
            new LocalizedString("options_mod_folder", false)
        ); OptionsMenuCreator.Item<Button> optimize = OptionsMenuCreator.CreateRow<Button> (
            container, OptionsMenuCreator.ItemType.BUTTON,
            null, new LocalizedString("options_mod_optimize", false)
        ); OptionsMenuCreator.Item<Button> clean = OptionsMenuCreator.CreateRow<Button>(
            container, OptionsMenuCreator.ItemType.BUTTON, 
            null, new LocalizedString("options_mod_clean", false)
        );

        optimize.itemClass.onClick.AddListener(() => {
            int index = modSelect.itemClass.value;
            OptimizeMod(options[index], props[index], !folder.itemClass.isOn, compress.itemClass.isOn);
        });
        clean.itemClass.onClick.AddListener(() => {
            int index = modSelect.itemClass.value;
            CleanMod(options[index], props[index]);
        });

        checkers.Add(new Checker(() => props[modSelect.itemClass.value].IsOptimized,
            (isOptimized) => {
                clean.container.gameObject.SetActive(isOptimized);
            }
        ));
    }

    void CreateToggleWatcher(bool enableWhenToggled, GameObject target, params Toggle[] toggles){
        if(toggles == null || toggles.Length == 0) return;
        checkers.Add(new ToggleWatcher(toggles.ToList(), enableWhenToggled, target));
    }

    void Update(){
        checkers.ForEach((w) => w.Update());
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
            string targetCopyPath = sameFolder ? fullSourceFolderPath : fullTargetFolderPath;
            if(folderName == "images"){
                string[] onlyExts = {".json"};
                if(!sameFolder) IOOperations.CopyFiles(fullSourceFolderPath, fullTargetFolderPath, onlyExts);
                ImageLoader.OptimizeImages(fullSourceFolderPath, targetCopyPath, compress);
            } else if(folderName == "locations"){
                string[] onlyExts = {".loc"};
                if(!sameFolder) IOOperations.CopyFiles(fullSourceFolderPath, fullTargetFolderPath, onlyExts);
                ColorRegionsLoader.OptimizeRegions(fullSourceFolderPath, targetCopyPath);
            } else if(!sameFolder){
                IOOperations.CopyFiles(fullSourceFolderPath, fullTargetFolderPath, null, ".meta");
            }
        }
        
        string propsPath = sameFolder ? fullPath : targetPath;
        GameLoader.Properties newProps = props.Clone();
        newProps.SetOptimized();
        System.IO.File.WriteAllText(propsPath+"/properties.json", JsonUtility.ToJson(newProps, true));

        CreateMenu();
    }

    void CleanMod(string modFolder, GameLoader.Properties props){
        string fullPath = string.Format("{0}/{1}/{2}",Application.streamingAssetsPath, GameLoader.path, modFolder);
        string[] imRemoveArr = {".png", ".jpg", ".jpeg"};
        string[] imCheckArr = {".rawtex", ".rawtex", ".rawtex"}; 
        IOOperations.RemoveFilesWithCheck(fullPath+"/images", imRemoveArr, imCheckArr);
        string[] colRegRemoveArr = {".png"};
        string[] colRegCheckArr = {".creg"};
        IOOperations.RemoveFilesWithCheck(fullPath+"/locations", colRegRemoveArr, colRegCheckArr);
    }
}
