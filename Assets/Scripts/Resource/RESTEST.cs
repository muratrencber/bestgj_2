using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RESTEST : MonoBehaviour
{
    [SerializeField] string path;
    [SerializeField] string sfxPath;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Texture2D tex;

    public void SetImage(){
        Sprite s =  ResourceManager.LoadAsset<Sprite>(path);
        sr.sprite = s;
        tex = s.texture;
    }


    public void SetSfx(){
        AudioClip ac = ResourceManager.LoadAsset<AudioClip>(sfxPath);
        Debug.Log(ac);
        Debug.Log(ac.length);
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
    }
}
