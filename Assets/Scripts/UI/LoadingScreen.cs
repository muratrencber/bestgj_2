using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;
    static LoadingScreen instance;
    static bool CheckInstance() {
        if(instance == null){
            LoadingScreen lscreen = GameObject.FindObjectOfType<LoadingScreen>(true);
            if(lscreen != null) instance = lscreen;
            else return false;
        }
        return true;
    }

    public static void ShowText(string txt){
        if(!CheckInstance()) return;
        if(!instance.gameObject.activeSelf) instance.gameObject.SetActive(true);
        instance.text.text = txt;
    }

    public static void Hide(){
        if(!CheckInstance()) return;
        if(instance.gameObject.activeSelf) instance.gameObject.SetActive(false);
    }
}
