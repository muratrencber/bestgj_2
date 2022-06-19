using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : InteractableBase
{

    private Vector3 inititalPos;
    [SerializeField]
    private GameObject hazne;
    private float speed = 6f;
    private bool Pressed = false;
    private Vector2 mousePos;

    public override void OnCursorDown() {

        Pressed = true;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
    public override void OnCursorUp() {

        Pressed = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }
    void Start() {

        inititalPos = transform.position;
    }
    void LateUpdate()
    {
        if(Pressed) {

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;// new Vector3(mousePos.x, mousePos.y, mousePos.z -4);
            if(Mathf.Abs(mousePos.x - hazne.transform.position.x) <= 0.1 &&Mathf.Abs(mousePos.y - hazne.transform.position.y) <= 0.1) {

                Destroy(gameObject, 0);
            }
            

        }
        else {

            //transform.position = inititalPos;
            transform.position = Vector3.Lerp(transform.position, inititalPos, Time.deltaTime * speed);
        }
    }
}
