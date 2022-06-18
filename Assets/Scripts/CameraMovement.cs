using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //[SerializeField]
    public GameObject[] scenes;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if(hit.collider != null)
            {
                
                if(hit.collider.tag == "0" || hit.collider.tag == "1"  || hit.collider.tag == "2")
                {
                    int index = Int32.Parse(hit.collider.tag);
                    transform.position = new Vector3(scenes[index].transform.position.x, scenes[index].transform.position.y, scenes[index].transform.position.z - 5);
                }
                
            }
        }
    }
}
