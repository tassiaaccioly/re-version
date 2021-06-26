using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator anim;
    private SpriteRenderer sprite;

    // keyboard interaction
    private string buttonPressed;

    public const string RIGHT = "right";
    public const string LEFT = "left";

    // movement
    public float moveSpeed;

    //ground checks
    // private Transform groundCheck;
    // private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // grounded = true;
        buttonPressed = null;
    }

    // Update is called once per frame
    void Update()
    {
        // grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        setAnimations();
    }

    private void FixedUpdate()
    {

        if(buttonPressed == RIGHT)
        {
            //to give it a sense of sliding on ice, use "ForceMode2d.Force"
            //rb2D.AddForce(new Vector2(moveSpeed, 0), ForceMode2D.Impulse);
            rb2D.velocity = new Vector2(moveSpeed, 0);
        }
        else if(buttonPressed == LEFT)
        {
            rb2D.velocity = new Vector2(-moveSpeed, 0);
        }
        else if(buttonPressed == null)
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }

        //getting player key presses and setting the direction of the movement 
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            buttonPressed = RIGHT;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            buttonPressed = LEFT;
        }
        else
        {
            buttonPressed = null;
        }
    }

    private void setAnimations()
    {
        anim.SetBool("isWalking", (rb2D.velocity.x != 0f));
    }
}
