using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowGamesUI : MonoBehaviour
{
    const float PROMPT_MIN_SECONDS = 0.5f;
    [SerializeField] string languageWindowKey;
    [SerializeField] Transform container;

    void Start(){
        ShowAll();
    }

    void ShowAll(){
        GameLoader.Properties[] props = GameLoader.LoadGames();
        container.DestroyChildren();
        foreach(GameLoader.Properties prop in props){
            OptionsMenuCreator.Item<Button> propButton = OptionsMenuCreator.CreateRow<Button>(
                container, OptionsMenuCreator.ItemType.BUTTON, null, new LocalizedString(prop.DisplayName));
            propButton.itemClass.onClick.AddListener(() => this.StartCoroutine(ResourceManager.LoadGame(prop, HandleException, ShowLanguages, true)));
        }
    }

    void ShowLanguages(){
        WindowChanger.ChangeWindow(languageWindowKey);
    }

    void HandleException(System.Exception e){
        LoadingScreen.Hide();
        Locales.TryGetLineMain("error_prompt", out string err, e.Message);
        Popup.Show(err);
    }
}
