using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ImageMagick;
using System;

[System.Serializable]
public class OptimizedImageList{
    public List<OptimizedImage> images = new List<OptimizedImage>();
}

[System.Serializable]
public class OptimizedImage{
    public string key;
    public int width;
    public int height;
    public TextureFormat format;
    public int mipCount;

    public OptimizedImage(){}
    public OptimizedImage(Texture2D tex, string key){
        this.width = tex.width;
        this.height = tex.height;
        this.key = key;
        this.format = tex.format;
        this.mipCount = tex.mipmapCount;
    }
}
public class ImageLoader
{
    public static bool compress = true;
    static ulong totalPixels;
    static Dictionary<string, OptimizedImage> optimizedImages = null;
    public static void LoadImages(string path, IDictionary itemsInterface){
        Dictionary<string, Sprite> items = itemsInterface as Dictionary<string, Sprite>;
        
        string[] extensions = {".png",".jpg",".jpeg",".bmp"};
        if(ResourceManager.loadingOptimized){
            optimizedImages = new Dictionary<string, OptimizedImage>();
            OptimizedImageList optimlist = JsonUtility.FromJson<OptimizedImageList>(File.ReadAllText(path+"/sizes.json"));
            foreach(OptimizedImage optim in optimlist.images)
                optimizedImages.TryAdd(optim.key, optim);
            string[] newExtensions = {".rawtex"};
            extensions = newExtensions;
        }
        
        StreamingAssetLoader<Sprite>.Properties defaults = new StreamingAssetLoader<Sprite>.Properties();
        defaults.AddProperty("maxSize", 2048);
        defaults.AddProperty("pixelsPerUnit", 100);
        StreamingAssetLoader<Sprite> sal = new StreamingAssetLoader<Sprite>(extensions, ProcessFile, defaults, items);
        sal.BeginLoad(path);

        if(optimizedImages != null){
            optimizedImages.Clear();
            optimizedImages = null;
        }
    }

    //for git commit
    static Sprite ProcessFile(string filePath,
                                string keyName,
                                StreamingAssetLoader<Sprite>.Properties p,
                                StreamingAssetLoader<Sprite>.PropertiesList pl,
                                StreamingAssetLoader<Sprite> sal){
        int maxSize = (int)sal.GetProperty("maxSize", pl, p).NumericValue;
        float pPerU = sal.GetProperty("pixelsPerUnit", pl, p).NumericValue;

        int w = 1; int h = 1;
        bool optimized = false;
        Texture2D targetTexture;
        if(optimizedImages != null && optimizedImages.TryGetValue(keyName, out OptimizedImage optim)){
            targetTexture = new Texture2D(optim.width, optim.height, optim.format, optim.mipCount, false);
            optimized = true;
        } else{
            targetTexture = new Texture2D(1,1,UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm, UnityEngine.Experimental.Rendering.TextureCreationFlags.Crunch);
        }

        byte[] imBytes = File.ReadAllBytes(filePath);
        if(compress && !optimized){
            MemoryStream outStream = new MemoryStream();
            FileInfo info = new FileInfo(filePath);
            using (MagickImage mi = new MagickImage(info)){
                mi.Quality = 75;
                pPerU *= TryResize(mi, maxSize);
                mi.Write(outStream);
            }
            targetTexture.LoadImage(outStream.ToArray());
            outStream.Close();
            if(targetTexture.width % 4 == 0 && targetTexture.height % 4 == 0)
                targetTexture.Compress(false);
        } else if(optimized){
            targetTexture.LoadRawTextureData(imBytes);
            targetTexture.Apply();
        }else{
            targetTexture.LoadImage(imBytes);
        }

        Sprite newSprite = Sprite.Create(targetTexture,
                                            new Rect(0,0,targetTexture.width,targetTexture.height),
                                            Vector2.one * 0.5f,
                                            pPerU);
        return newSprite;
    }

    static float TryResize(MagickImage mi, int maxSize){
        int maxTex = Mathf.Max(mi.Width, mi.Height);
        float ratio = (float)maxSize / (float) maxTex;
        if(ratio >= 1)
            return 1;
        int targetX = Mathf.RoundToInt((float)mi.Width * ratio);
        int targetY = Mathf.RoundToInt((float)mi.Height * ratio);
        mi.Resize(targetX, targetY);
        return ratio;
    }

    public static void OptimizeImages(string sourcePath, string targetPath, bool cmprs){
        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        bool savedCompress = compress;
        compress = cmprs;
        LoadImages(sourcePath, sprites);
        compress = savedCompress;

        OptimizedImageList optimList = new OptimizedImageList();
        
        IOOperations.WriteFromKeys<Sprite>(sprites, sourcePath, (p, kvp)=>{
            Texture2D tex = kvp.Value.texture;
            OptimizedImage optim = new OptimizedImage(tex, kvp.Key);
            optimList.images.Add(optim);

            File.WriteAllBytes(targetPath+"/"+kvp.Key+".rawtex", tex.GetRawTextureData());
        });

        string optimListJSON = JsonUtility.ToJson(optimList, true);
        File.WriteAllText(targetPath+"/sizes.json", optimListJSON);
    }
}
