using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Player : MonoBehaviour
{
    int total = 0;
    int money;

    [SerializeField]
    private GameObject computer;
    [SerializeField] 
    TextMeshProUGUI score;
    void Start() {

        money = 1000;
        score.text = "Score: " + total;
    }
    void Update()
    {
        //GameObject score = GameObject.FindGameObjectWithTag("PlayerScore");
        
        if (Input.GetMouseButtonDown(0))
        {
            
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            
            if(hit.collider != null)
            {
                //Debug.Log(5);
                if(hit.collider.tag == "card")
                {
                    GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("card");
                    var hitted = (GameObject)hit.collider.gameObject;
                    GameObject otherGameObject;
                    if(hitted == gameObjects[0]) {

                        otherGameObject = gameObjects[1];
                    }else {
                        otherGameObject = gameObjects[0];
                    }
                    /*var gameObject = (GameObject)hit.collider.gameObject;
                    total += gameObject.GetComponent<Card>().point;
                    score.text = "Score: " + total;
                    Destroy(gameObject, 0);
                    

                    GameObject otherCard = GameObject.FindGameObjectWithTag("card");
                    computer.GetComponent<Computer>().total += otherCard.GetComponent<Card>().point;
                    Destroy(otherCard, 0.1f);
                    Debug.Log(computer.GetComponent<Computer>().total);*/

                    total += hitted.GetComponent<Card>().point;
                    score.text = "Score: " + total;
                    Destroy(hitted, 0);

                    computer.GetComponent<Computer>().total += otherGameObject.GetComponent<Card>().point;
                    Destroy(otherGameObject, 0.1f);
                    Debug.Log(computer.GetComponent<Computer>().total);
                    
                }
                
            }
        }
    }

    
}
