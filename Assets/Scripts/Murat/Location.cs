using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    static Dictionary<string, Location> locations = new Dictionary<string, Location>();
    [SerializeField] string key;

    void Awake(){
        Register();
    }

    public static Location GetLocation(string key){
        if(locations.TryGetValue(key, out Location loc))
            return loc;
        return null;
    }
    void Register(){
        locations.Add(this.key, this);
    }
}
