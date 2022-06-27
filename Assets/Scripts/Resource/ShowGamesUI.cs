using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowGamesUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] string buttonPath = "UI/GameSelectButton";

    void Start(){
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
            if(b) b.onClick.AddListener(() => ResourceManager.LoadGame(prop.GamePath));
        }
    }
}
