using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithAutomata : MonoBehaviour
{
    [SerializeField]
    GameObject automata;
    /*[SerializeField]
    GameObject[] items;*/
    //private Collider2D prefab;
    float xAxis;
    Ray ray;
    //RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        xAxis = automata.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(transform.position.x - xAxis) <= 0.4f)
        {
            if (Input.GetMouseButtonDown(0))
            {

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
                if(hit2D.collider != null)
                {
                    if(hit2D.collider.tag == "Probis" || hit2D.collider.tag == "Canpare" || hit2D.collider.tag == "Cizivic" || hit2D.collider.tag == "Kraker" || hit2D.collider.tag == "Tutku")
                    {
                        /*prefab = hit2D.collider;
                        prefab.GetComponent<Rigidbody2D>().gravityScale = 1;*/
                        Instantiate(hit2D.collider, new Vector3(0.3f, -1, 0), Quaternion.identity);
                    }
                    //Debug.Log(hit2D.collider.transform.position.x);
                }
            }
        }

    }

    /*public void DetectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay
        RaycastHit2D hit2D = Physics2D.GetRayIntersection()
    }*/
}
