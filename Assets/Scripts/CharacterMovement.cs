using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //[SerializeField]
    private float speed = 1.2f;
    private float force = 3.0f;
    private float horizontalInput;
    //private bool isJumping = false;
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject ground;
    [SerializeField]
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        horizontalInput = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput * speed));
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);

       
        if (Input.GetButtonDown("Jump") && Mathf.Abs(transform.position.y - ground.transform.position.y) <= 0.01f)
        {
            rb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        } 
        else if(!Input.GetButtonDown("Jump") && Mathf.Abs(transform.position.y - ground.transform.position.y) <= 0.001f)
        {
            animator.SetBool("IsJumping", false);
        }

        if(horizontalInput * transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

}
