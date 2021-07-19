using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Black : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator anim;
    private float xAxis;

    private bool facingRight;


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        xAxis = Input.GetAxisRaw("Horizontal");

        if ((xAxis < 0f && facingRight) || (xAxis > 0f && !facingRight))
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
