using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class CloneDirectory
{
    public static void Start(string rootParentPath, string targetParentPath, string[] onlyFileTypes, params string[] excludedFileTypes){
        string sourceDir = rootParentPath;
        string targetDir = targetParentPath;

        Queue<string> remainingDirectories = new Queue<string>();
        remainingDirectories.Enqueue(sourceDir);

        while(remainingDirectories.Count > 0){
            string targetPath = remainingDirectories.Dequeue();
            string directoryInTarget = targetDir + targetPath.Substring(targetPath.IndexOf(sourceDir)+sourceDir.Length);
            if(!Directory.Exists(directoryInTarget)) Directory.CreateDirectory(directoryInTarget);

            string[] subDirectories = Directory.GetDirectories(targetPath);
            foreach(string s in subDirectories){
                string adjustedPath = s.Replace("\\", "/");
                remainingDirectories.Enqueue(adjustedPath);
            }

            string[] targetPaths = Directory.GetFiles(targetPath);

            foreach(string p in targetPaths){
                string adjustedPath = p.Replace("\\", "/");

                if(onlyFileTypes != null && !onlyFileTypes.Contains(Path.GetExtension(p))) continue;
                if(excludedFileTypes != null && excludedFileTypes.Contains(Path.GetExtension(p))) continue;

                string relativePath = adjustedPath.Substring(adjustedPath.IndexOf(sourceDir)+sourceDir.Length+1);
                string pathToWrite = string.Format("{0}/{1}", targetDir, relativePath);

                File.Copy(adjustedPath, pathToWrite, true);
            }
        }
    }
}
