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
    public bool canMove;
    public float moveSpeed;
    public float runSpeed;
    private bool facingRight;
    private float xAxis;

    //jump
    private bool isJumping;
    public int jumpForce;

    //ground checks
    public Transform groundCheck;
    private bool isGrounded;

    //stamina
    public int totalStamina;
    private float actualStamina;


    //memories
    private int totalOfMemories = 12;
    private int totalOfAbilityMemories = 0;
    private string[] abilityMemories;
    private int totalOfFullMemories = 0;
    private string[] fullMemories;
    private string memoryChip;

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
        abilityMemories = new string[totalOfMemories];
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        xAxis = Input.GetAxisRaw("Horizontal");

        if(!canMove)
        {
            rb2D.velocity =  new Vector2(0, 0);
            ChangeAnimationState(PLAYER_IDLE);
            return;
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

    private void FixedUpdate()
    {
        if(!canMove)
        {
            rb2D.velocity = new Vector2(0, 0);
            ChangeAnimationState(PLAYER_IDLE);
            return;
        }

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

        //run
        if(totalOfAbilityMemories > 0)
        {
            moveSpeed = runSpeed;
        }

        //jump
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetButtonDown("Jump") && isGrounded && !isJumping)
        {
            isJumping = true;
            isGrounded = false;
            ChangeAnimationState(PLAYER_JUMPUP);
            StartCoroutine(JumpAnimation());
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
        yield return new WaitForSeconds(.8f);
        if(facingRight)
        {
            transform.position = new Vector3(transform.position.x + .1f, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - .1f, transform.position.y, transform.position.z);
        }
        yield return new WaitForSeconds(.6f);
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
        if(other.CompareTag("AbilityMemoryChip"))
        {
            totalOfAbilityMemories++;
            Debug.Log("Total of Ability Memories: " + totalOfAbilityMemories);
            memoryChip = FindObjectOfType<MemoryChip>().memoryTag;
            Debug.Log(memoryChip);
            abilityMemories.SetValue(memoryChip, totalOfAbilityMemories - 1);
            Debug.Log(abilityMemories[0]); 
        }
    }
}