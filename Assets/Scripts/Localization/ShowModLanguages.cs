using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
            OptionsMenuCreator.Item<Button> propButton = OptionsMenuCreator.CreateRow<Button>(
                container, OptionsMenuCreator.ItemType.BUTTON,
                null, new LocalizedString(loc.Name));
            propButton.itemClass.onClick.AddListener(() => {
                Locales.SetModKey(loc.LanguageKey);
                SceneManager.LoadScene("CustomGame");
            });
        }
    }
}
