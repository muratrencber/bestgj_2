using UnityEngine;

public class WindowChanger : MonoBehaviour
{
    [SerializeField] string key;
    public void Change(){
        UIWindow.ChangeToWindow(key);
    }
}
