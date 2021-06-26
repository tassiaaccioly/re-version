using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator anim;
    private SpriteRenderer sprite;

    // keyboard interaction
    private string buttonPressed;

    const string RIGHT = "right";
    const string LEFT = "left";

    // movement
    public float moveSpeed;
    private bool facingRight;
    private float xAxis;

    //ground checks
    private Transform groundCheck;
    private bool isGrounded;

    // Animation States
    private string currentAnimation;
    const string PLAYER_IDLE = "Player_Idle";
    const string PLAYER_WALK = "Player_Walk";
    const string PLAYER_RUN = "Player_Run";


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isGrounded = true;
        buttonPressed = null;
        facingRight = true;
        // groundCheck - 1 << LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

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

    private void FixedUpdate()
    {
        if (buttonPressed == RIGHT)
        {
            rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y);
        }
        else if (buttonPressed == LEFT)
        {
            rb2D.velocity = new Vector2(-moveSpeed, rb2D.velocity.y);
        }
        else if (buttonPressed == null)
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }

        if ((xAxis < 0f && facingRight) || (xAxis > 0f && !facingRight))
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        Debug.Log(rb2D.velocity.x);

        //Animation change

        if(xAxis != 0f)
        {
            ChangeAnimationState(PLAYER_WALK);
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    void ChangeAnimationState(string newAnimation)
    {
        //stop the same animation from interrupting itself
        if (currentAnimation == newAnimation) return;

        //play the animation
        anim.Play(newAnimation);

        //reassign the current state
        currentAnimation = newAnimation;
    }
}