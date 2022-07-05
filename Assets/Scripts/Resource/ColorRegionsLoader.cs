using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ColorRegion{
    public Color color;
    public int topLeft, bottomRight;
    public Vector2 adjustedTopLeft, adjustedBottomRight;

    public void CreateRect(int width, int height){
        adjustedTopLeft = ToPos(topLeft, width, height);
        adjustedBottomRight = ToPos(bottomRight, width, height);
    }

    Vector2 ToPos(int index, int width, int height){
        int x = index % width;
        int y = index / width;
        float xRatio = (float)x/(float)width;
        float yRatio = (float)y/(float)height;
        return new Vector2(xRatio, yRatio);
    }
}
public class ColorRegionsLoader
{
    const long MAX_SIZE = 20000;
    public static void LoadRegions(string path, Dictionary<string, List<ColorRegion>> items){
        string[] extensions = {".png"};
        StreamingAssetLoader<List<ColorRegion>>.Properties defaults = new StreamingAssetLoader<List<ColorRegion>>.Properties();
        StreamingAssetLoader<List<ColorRegion>> sal = new StreamingAssetLoader<List<ColorRegion>>(extensions, ProcessFile, defaults, items);
        sal.BeginLoad(path);
    }
    static List<ColorRegion> ProcessFile(string filePath,
                                string keyName,
                                StreamingAssetLoader<List<ColorRegion>>.Properties p,
                                StreamingAssetLoader<List<ColorRegion>>.PropertiesList pl,
                                StreamingAssetLoader<List<ColorRegion>> sal){
        List<ColorRegion> regList = new List<ColorRegion>();
        Texture2D targetTexture = new Texture2D(1, 1, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm, UnityEngine.Experimental.Rendering.TextureCreationFlags.Crunch);
        byte[] imBytes = File.ReadAllBytes(filePath);
        targetTexture.LoadImage(imBytes);
        long size = targetTexture.width * targetTexture.height;
        if(size > MAX_SIZE){
            throw new System.Exception(string.Format("File: {0}\n Color region size too big ({1})! (MAX SIZE: {2})", filePath, size, MAX_SIZE));
        }
        Color[] cols = targetTexture.GetPixels();
        for(int i = 0; i < cols.Length; i++){
            Color c = cols[i];
            if(c.a < 1) continue;
            ColorRegion cr = new ColorRegion();
            cr.color = c;
            cr.topLeft = i;
            cr.bottomRight = i;
            Queue<int> neighbours = new Queue<int>();
            neighbours.Enqueue(i);
            while(neighbours.Count > 0){
                int currentIndex = neighbours.Dequeue();
                AddNeighbours(currentIndex, c, cols, neighbours, targetTexture.width, targetTexture.height, cr);
                cols[currentIndex] = new Color(0,0,0,0);
            }
            cr.CreateRect(targetTexture.width, targetTexture.height);
            regList.Add(cr);
        }
        return regList;
    }

    static void AddNeighbours(int index, Color c, Color[] arr, Queue<int> neighbours, int width, int height, ColorRegion cr){
        int x = index % width;
        int y = index / width;
        List<int> colsToCheck = new List<int>();
        if(x > 0) colsToCheck.Add(index - 1);
        if(x < width - 1) colsToCheck.Add(index + 1);
        if(y > 0) colsToCheck.Add(index - width);
        if(y < height - 1) colsToCheck.Add(index + width);
        foreach(int tempIndex in colsToCheck){
            if(arr[tempIndex] == c){
                if(tempIndex < cr.topLeft) cr.topLeft = tempIndex;
                if(tempIndex > cr.bottomRight) cr.bottomRight = tempIndex;
                arr[tempIndex] = new Color(0,0,0,0);
                neighbours.Enqueue(tempIndex);
            }
        } 
    }
}
