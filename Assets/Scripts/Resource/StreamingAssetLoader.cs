using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class StreamingAssetLoader<T>
{
    [System.Serializable]
    public class Property{
        public float NumericValue{get{return numericValue;}} 
        public string StringValue {get{return stringValue;}}
        public string Key {get{return key;}}
        [SerializeField] string key;
        [SerializeField] float numericValue = 0;
        [SerializeField] string stringValue = "";

        public Property(string key, float numericValue, string stringValue){
            this.key = key;
            this.numericValue = numericValue;
            this.stringValue = stringValue;
        }
    }
    [System.Serializable]
    public class Properties{
        public string FileName{get{return fileName;}}
        [SerializeField] List<Property> properties = new List<Property>();
        [SerializeField] string fileName;

        public Property GetProperty(string key){
            IEnumerable<Property> props = properties.Where((p) => p.Key == key);
            return props.FirstOrDefault();
        }
        public void AddProperty(string key, float numericValue = 0, string stringValue = ""){
            if(HasProperty(key)) return;
            properties.Add(new Property(key, numericValue, stringValue));
        }
        public void AddProperty(string key, string stringValue) => AddProperty(key, 0, stringValue);
        public bool HasProperty(string key) => GetProperty(key) != null;
    }

    [System.Serializable]
    public class PropertiesList{
        public Properties Default {get{return defaults;}}
        public List<Properties> Properties {get{return properties;}}
        [SerializeField] Properties defaults;
        [SerializeField] List<Properties> properties = new List<Properties>();
    }
    Properties defaultProperties;
    string[] supportedExtensions;
    string mainPath;
    string propertiesName = "properties.json";

    Dictionary<string, T> items;
    Dictionary<string, PropertiesList> dirProperties = new Dictionary<string, PropertiesList>();
    Dictionary<string, Properties> fileProperties = new Dictionary<string, Properties>();
    public delegate T PathProcessor(string fP, string kN, Properties p, PropertiesList pl, StreamingAssetLoader<T> sal);
    PathProcessor callback;

    public StreamingAssetLoader(string[] supportedExtensions, PathProcessor pp, Properties defaultProperties, Dictionary<string, T> items){
        this.supportedExtensions = supportedExtensions;
        this.callback = pp;
        this.defaultProperties = defaultProperties;
        this.items = items;
        this.propertiesName = "properties.json";
    }

    public void SetPropertiesFileName(string newName) => propertiesName = newName;
    protected bool IsSupported(string filePath) => supportedExtensions.Contains(Path.GetExtension(filePath));
    public void BeginLoad(string path){
        mainPath = path;
        LoadAllInDirectory();
    }

    protected void LoadProperties(string directoryPath){
        string propsPath = directoryPath+"/"+propertiesName;
        if(!File.Exists(propsPath)) return;
        string contents = File.ReadAllText(propsPath);
        PropertiesList pl = JsonUtility.FromJson<PropertiesList>(contents);
        if(!dirProperties.ContainsKey(directoryPath)) dirProperties.Add(directoryPath, pl);
        foreach(Properties p in pl.Properties){
            string k = directoryPath + "/"+p.FileName;
            if(!fileProperties.ContainsKey(k))
                fileProperties.Add(k, p);
            fileProperties[k] = p;
        }
    }

    protected KeyValuePair<Properties, PropertiesList> GetPropsForFile(string dir, string filePath){
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string targetPropsKey = dir+"/"+fileName;
        bool hasProperties = fileProperties.TryGetValue(targetPropsKey, out Properties p);
        bool hasPropList = dirProperties.TryGetValue(dir, out PropertiesList pl);
        Properties returnProps = hasProperties ? p : null;
        PropertiesList returnPropsList = hasPropList ? pl : null;
        return new KeyValuePair<Properties, PropertiesList>(returnProps, returnPropsList);
    }

    public Property GetProperty(string key, PropertiesList pl, Properties p){
        Property contender = null;
        if(p != null) contender = p.GetProperty(key);
        if(contender != null) return contender;
        if(pl != null && pl.Default != null) contender = pl.Default.GetProperty(key);
        if(contender != null) return contender;
        return defaultProperties.GetProperty(key);
    }

    void LoadAllInDirectory(){
        Queue<string> dirPaths = new Queue<string>();
        string directoryPath = mainPath.Replace("\\","/");
        System.Action<string> processDirectory = (string directoryPath) => {
            if(!Directory.Exists(directoryPath)) throw new System.Exception("Directory " +directoryPath+ " does not exist!");
            LoadProperties(directoryPath);
        };
        System.Action<string, string> processFile = (string filePath, string dirPath) => {
            string keyName = dirPath+"/"+Path.GetFileNameWithoutExtension(filePath);
            if(items.ContainsKey(keyName)) return;
            keyName = keyName.Split(directoryPath+"/")[1];
            KeyValuePair<Properties, PropertiesList> pAndPl = GetPropsForFile(directoryPath, filePath);
            T result = callback(filePath, keyName, pAndPl.Key, pAndPl.Value, this);
            if(result != null) items.Add(keyName, result);
        };
        IOOperations.SearchAllFiles(directoryPath, processDirectory, processFile, supportedExtensions);
    }
}