using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 playerCurPos;

    //memories
    private MemoryController memoryController;
    private int totalOfAbilityMemories = 0;

    // keyboard interaction
    private string buttonPressed;
    const string RIGHT = "right";
    const string LEFT = "left";

    // movement
    public bool canMove;
    public float moveSpeed;
    public float runSpeed;
    private bool facingRight;
    private float xAxis;

    //jump
    private bool canJump;
    private bool isJumping;
    public float jumpHeight;

    //ground checks
    public Transform groundCheck;
    private bool isGrounded;

    // collider checks
    private RaycastHit2D playerJumpCheck;
    public Vector2 startJumpCast;
    public Vector2 endJumpCast;

    //stamina
    public int totalStamina;
    private float actualStamina;

    // Animation States
    private string currentAnimation;
    const string PLAYER_IDLE = "Player_Idle";
    const string PLAYER_WALK = "Player_Walk";
    const string PLAYER_RUN = "Player_Run";
    const string PLAYER_JUMPUP = "Player_JumpUp";
    const string PLAYER_JUMPDOWN = "Player_JumpDown";

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isGrounded = true;
        buttonPressed = null;
        facingRight = true;
        canMove = true;
        isJumping = false;
        canJump = true;
        memoryController = FindObjectOfType<MemoryController>();
    }

    // Update is called once per frame
    void Update()
    {

        // store player current position into a variable
        playerCurPos = rb2D.transform.position;

        // lines cast from player
        startJumpCast = playerCurPos;
        startJumpCast.x += .3f;
        startJumpCast.y -= .4f;
        endJumpCast = startJumpCast;
        endJumpCast.x += 2f;

        Debug.DrawLine(startJumpCast, endJumpCast, Color.red);

        playerJumpCheck = Physics2D.Linecast(startJumpCast, endJumpCast);

        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        xAxis = Input.GetAxis("Horizontal");

        if (!canMove)
        {
            rb2D.velocity =  new Vector2(0, 0);
            ChangeAnimationState(PLAYER_IDLE);
            return;
        }

        if (totalOfAbilityMemories > 0)
        {
            moveSpeed = runSpeed;
            canJump = true;
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

        //if the player got the first memory they can walk faster/run and jump
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetButtonDown("Jump"))
        {
            Debug.Log("I got here");
            Debug.Log("canJump: " + canJump);
            Debug.Log("isGrounded: " + isGrounded);

            if (canJump && isGrounded)
            {
                isJumping = true;
                isGrounded = false;
            }
            
        }

    }

    private void FixedUpdate()
    {
        if(!canMove)
        {
            rb2D.velocity = new Vector2(0, 0);
            ChangeAnimationState(PLAYER_IDLE);
            return;
        }

        if (!isJumping)
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
        }

        if (isJumping)
        {
            rb2D.velocity = new Vector2(0, 0);
            ChangeAnimationState(PLAYER_JUMPUP);
            StartCoroutine(JumpAnimation());
        }

        // turn player sprite to face input
        if ((xAxis < 0f && facingRight) || (xAxis > 0f && !facingRight))
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        //Animation change

        if (xAxis != 0f && totalOfAbilityMemories > 0)
        {
            ChangeAnimationState(PLAYER_RUN);
        }
        else if (xAxis != 0f)
        {
            ChangeAnimationState(PLAYER_WALK);
        }
        else if (rb2D.velocity == new Vector2(0f, 0f) && !isJumping)
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    IEnumerator JumpAnimation()
    {
        yield return new WaitForSeconds(.5f);
        if(facingRight)
        {
            transform.position = new Vector3(transform.position.x + .01f, transform.position.y + jumpHeight, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - .01f, transform.position.y + jumpHeight, transform.position.z);
        }
        ChangeAnimationState(PLAYER_JUMPDOWN);
        yield return new WaitForSeconds(.9f);
        isJumping = false;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        memoryController = FindObjectOfType<MemoryController>();

        if (other.CompareTag("AbilityMemoryChip"))
        {
            totalOfAbilityMemories++;
            memoryController.addAbilityMemory(other.GetComponent<MemoryChip>().memoryTag);
        }

        if (other.CompareTag("FullMemoryChip"))
        {
            memoryController.addFullMemory(other.GetComponent<MemoryChip>().memoryTag);
        }

        if (other.CompareTag("BrokenMemoryChip"))
        {
            memoryController.addBrokenMemory(other.GetComponent<MemoryChip>().memoryTag);
        }
    }
}