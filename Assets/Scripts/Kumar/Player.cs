using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int total = 0;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if(hit.collider != null)
            {
                
                if(hit.collider.tag == "card")
                {
                    
                    //GameObject card = hit.collider;
                    Destroy(hit.collider, 1);
                    //transform.position = new Vector3(scenes[index].transform.position.x, scenes[index].transform.position.y, scenes[index].transform.position.z - 5);
                }
                
            }
        }
    }

    
}
