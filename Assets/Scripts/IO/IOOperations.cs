using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using UnityEngine;

public static class IOOperations
{
    public static void CopyFiles(string rootParentPath, string targetParentPath, string[] onlyFileTypes, params string[] excludedFileTypes){
        string sourceDir = rootParentPath;
        string targetDir = targetParentPath;

        Action<string> processDirectory = (dirPath) => {
            string directoryInTarget = targetDir + dirPath.Substring(dirPath.IndexOf(sourceDir)+sourceDir.Length);
            if(!Directory.Exists(directoryInTarget)) Directory.CreateDirectory(directoryInTarget);
        };

        Action<string, string> processFile = (adjustedPath, dirPath) => {
            string relativePath = adjustedPath.Substring(adjustedPath.IndexOf(sourceDir)+sourceDir.Length+1);
            string pathToWrite = string.Format("{0}/{1}", targetDir, relativePath);

            File.Copy(adjustedPath, pathToWrite, true);
        };

        SearchAllFiles(sourceDir, processDirectory, processFile, onlyFileTypes, excludedFileTypes);
    }

    public static void RemoveFiles(string rootPath, string[] onlyFileTypes, params string[] excludedFileTypes){
        Action<string, string> processFile = (path, dirPath) => {
            if(!File.Exists(path)) return;
            File.Delete(path);
        };
        SearchAllFiles(rootPath, null, processFile, onlyFileTypes, excludedFileTypes);
    }

    public static void RemoveFilesWithCheck(string rootPath, string[] extensions, string[] compareExtensions){
        Action<string, string> processFile = (path, dirPath) => {
            if(!File.Exists(path)) return;
            string ext = Path.GetExtension(path);
            string targetExtension = compareExtensions[extensions.ToList().IndexOf(ext)];
            if(!File.Exists(dirPath+"/"+Path.GetFileNameWithoutExtension(path)+targetExtension)) return;
            File.Delete(path);
            if(File.Exists(path+".meta")) File.Delete(path+".meta");
        };
        
        SearchAllFiles(rootPath, null, processFile, extensions);
    }

    public static void WriteFromKeys<T>(Dictionary<string, T> collection, string path, Action<string, KeyValuePair<string, T>> processFile){
        foreach(KeyValuePair<string, T> kvp in collection){

            string fullTargetDir =  path;
            if(kvp.Key.Contains("/")){
                string withoutFileName = kvp.Key.Substring(0, kvp.Key.LastIndexOf("/"));
                fullTargetDir += "/"+withoutFileName;
            }

            if(!Directory.Exists(fullTargetDir)) Directory.CreateDirectory(fullTargetDir);
            processFile?.Invoke(fullTargetDir, kvp);
        }
    }

    public static void SearchAllFiles(string rootPath, Action<string> processDirectory, Action<string, string> processFile, string[] onlyFileTypes, params string[] excludedFileTypes){
        Queue<string> remainingDirectories = new Queue<string>();
        remainingDirectories.Enqueue(rootPath);

        while(remainingDirectories.Count > 0){
            string targetPath = remainingDirectories.Dequeue();
            processDirectory?.Invoke(targetPath);

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

                processFile?.Invoke(adjustedPath, targetPath);
            }
        }
    }
}
