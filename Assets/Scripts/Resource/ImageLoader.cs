using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageLoader
{
    static ulong totalPixels;
    public static void LoadImages(string path, Dictionary<string, Sprite> items){
        string[] extensions = {".png",".jpg",".jpeg",".bmp"};
        StreamingAssetLoader<Sprite>.Properties defaults = new StreamingAssetLoader<Sprite>.Properties();
        defaults.AddProperty("maxSize", 2048);
        defaults.AddProperty("pixelsPerUnit", 100);
        StreamingAssetLoader<Sprite> sal = new StreamingAssetLoader<Sprite>(extensions, ProcessFile, defaults, items);
        sal.BeginLoad(path);
        Debug.Log(totalPixels);
    }
    static Sprite ProcessFile(string filePath,
                                string keyName,
                                StreamingAssetLoader<Sprite>.Properties p,
                                StreamingAssetLoader<Sprite>.PropertiesList pl,
                                StreamingAssetLoader<Sprite> sal){
        int maxSize = (int)sal.GetProperty("maxSize", pl, p).NumericValue;
        float pPerU = sal.GetProperty("pixelsPerUnit", pl, p).NumericValue;
        Texture2D targetTexture = new Texture2D(1, 1, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm, UnityEngine.Experimental.Rendering.TextureCreationFlags.Crunch);
        byte[] texBytes = File.ReadAllBytes(filePath);
        UnityEditor.TextureImporter textureImporter = new UnityEditor.TextureImporter();
        
        Resize(targetTexture, maxSize);
        //targetTexture.Compress(false);
        targetTexture.Apply();
        Debug.LogFormat("width: {0}, height: {1}", targetTexture.width, targetTexture.height);

        totalPixels += (ulong)(targetTexture.width * targetTexture.height);
        Sprite newSprite = Sprite.Create(targetTexture,
                                            new Rect(0,0,targetTexture.width,targetTexture.height),
                                            Vector2.one * 0.5f,
                                            pPerU);
        return newSprite;
    }
    static void Resize(Texture2D texture2D, int maxSize)
    {
        int maxTex = Mathf.Max(texture2D.width, texture2D.height);
        float ratio = (float)maxSize / (float) maxTex;
        if(ratio >= 1)
            return;
        int firstX = texture2D.width;
        int firstY = texture2D.height;
        int targetX = Mathf.RoundToInt((float)texture2D.width * ratio);
        int targetY = Mathf.RoundToInt((float)texture2D.height * ratio);
        TextureScale.Bilinear(texture2D, targetX, targetY);
        TextureScale.Bilinear(texture2D, firstX, firstY);
    }
}
