using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithAutomata : MonoBehaviour
{
    [SerializeField]
    GameObject automata;

    float xAxis;
    Ray ray;
    void Start()
    {
        xAxis = automata.transform.position.x;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if(hit.collider != null || hit.collider.tag == "button")
            {
                automata.GetComponent<Otomat>().enterItem(hit.collider.gameObject.GetComponent<OtomatButton>().number);
            }
        }

    }

}
