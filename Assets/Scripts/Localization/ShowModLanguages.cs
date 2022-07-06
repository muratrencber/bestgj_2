using UnityEngine;

public class ShowModLanguages : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] string buttonPath;
    void OnEnable(){
        ShowLanguages();
    }

    void ShowLanguages(){
        container.DestroyChildren();

        GameObject prefab = Resources.Load<GameObject>(buttonPath);
        Locales[] locs = ResourceManager.GetAll<Locales>();

        foreach(Locales loc in locs){
            GameObject prefabInstance = Instantiate(prefab, container);
            TMPro.TextMeshProUGUI text = prefabInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            UnityEngine.UI.Button b = prefabInstance.GetComponent<UnityEngine.UI.Button>();
            if(text) text.text = loc.Name;
            if(b) b.onClick.AddListener(() => {

            });
        }
    }
}
