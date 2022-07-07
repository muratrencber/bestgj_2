using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RESTEST : MonoBehaviour
{
    [SerializeField] string path;
    [SerializeField] string sfxPath;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] List<Texture2D> texes = new List<Texture2D>();

    public void SetImage(){
        Sprite s =  ResourceManager.LoadAsset<Sprite>(path);
        sr.sprite = s;
        Sprite[] allsss = ResourceManager.GetAll<Sprite>();
        foreach(Sprite sn in allsss) texes.Add(sn.texture);
    }


    public void SetSfx(){
        AudioClip ac = ResourceManager.LoadAsset<AudioClip>(sfxPath);
        Debug.Log(ac);
        Debug.Log(ac.length);
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
    }
}
