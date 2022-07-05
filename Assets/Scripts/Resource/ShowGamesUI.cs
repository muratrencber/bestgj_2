using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowGamesUI : MonoBehaviour
{
    const float PROMPT_MIN_SECONDS = 0.5f;

    [SerializeField] Transform container;
    [SerializeField] GameObject loadUI, popupUI;
    [SerializeField] TMPro.TextMeshProUGUI loadText, popupText;
    [SerializeField] string buttonPath = "UI/GameSelectButton";

    void Start(){
        popupUI.SetActive(false);
        loadUI.SetActive(false);
        ShowAll();
    }

    void ShowAll(){
        GameLoader.Properties[] props = GameLoader.LoadGames();
        GameObject prefab = Resources.Load<GameObject>(buttonPath);
        container.DestroyChildren();
        foreach(GameLoader.Properties prop in props){
            GameObject prefabInstance = Instantiate(prefab, container);
            TextMeshProUGUI text = prefabInstance.GetComponentInChildren<TextMeshProUGUI>();
            Button b = prefabInstance.GetComponent<Button>();
            if(text) text.text = prop.GameName;
            if(b) b.onClick.AddListener(() => this.StartCoroutine(ResourceManager.LoadGame(prop.GamePath, loadUI, loadText, HandleException)));
        }
    }

    void HandleException(System.Exception e){
        loadUI.SetActive(false);
        popupUI.SetActive(true);
        popupText.text = "ERROR: "+e.Message;
    }
}
