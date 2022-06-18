using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    static CursorManager cm;

    [System.Serializable]
    struct CursorSprite{
        public Sprite sprite;
        public string key;
        public Vector2 offset;
    }
    [SerializeField] CursorSprite[] sprites;
    [SerializeField] string defaultKey;
    Dictionary<string, CursorSprite> cursors = new Dictionary<string, CursorSprite>();

    void Awake(){
        CreateDict();
        cm = this;
        Set(defaultKey);
    }

    void CreateDict(){
        foreach(CursorSprite cs in sprites){
            cursors.Add(cs.key, cs);
        }
    }
    public static void SetDefault(){
        Set(cm.defaultKey);
    }
    public static void Set(string key){
        if(cm.cursors.TryGetValue(key, out CursorSprite cursorSpr)){
            Cursor.SetCursor(cursorSpr.sprite.texture, cursorSpr.offset, CursorMode.ForceSoftware);
        }
    }
}
