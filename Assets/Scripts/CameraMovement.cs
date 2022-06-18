using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //[SerializeField]
    //public IDictionary<string, GameObject> scenes = new Dictionary<string, GameObject>();
    
    // Start is called before the first frame update
    /*void Start()
    {
        
        scenes.Add("MainScene", null);
        scenes.Add("Scene1", null);
        scenes.Add("Scene2", null);
    }*/

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if(hit.collider != null)
            {
                if(hit.collider.tag == "MainScene" || hit.collider.tag == "Scene1"  || hit.collider.tag == "Scene2")
                {
                    Debug.Log(hit.collider.tag);
                }
                
            }
        }
    }
}
