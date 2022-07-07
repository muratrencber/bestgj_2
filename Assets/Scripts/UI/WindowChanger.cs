using UnityEngine;

public class WindowChanger : MonoBehaviour
{
    [SerializeField] string key;
    public void Change() => ChangeWindow(key);
    public static void ChangeWindow(string key){
        WindowList[] lists = GameObject.FindObjectsOfType<WindowList>(true);
        foreach(WindowList wl in lists){
            if(wl.TrySetWindow(key)) break;
        }
    }
}
