using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    const float speed = 10f;
    const float jumpPower = 12f;
    const float pogoJumpPower = 6f;
    public static float playerHealth;


    private Rigidbody2D playerRb;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private InputSystem inputSystem;

    private int maxJump = 2;
    private int jumpCount;
    private bool isMoving;
    private bool jumpBuffered;
    private float xRange = 8.36f;
    private float maxVelocityY = 20f;
    private float verticalInput;
    private float fallingGravity = 5;
    private float initialGravity;
    private float jumpBufferTimer = 0.1f;
    private float horizontalInput;
    private float jumpBufferCounter;
    private bool jumpButtonReleased = true;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        inputSystem = new InputSystem();

        inputSystem.Player.Enable();
        inputSystem.Player.Jump.started += Jump;
        playerHealth = 3;

        jumpCount = 0;
        initialGravity = playerRb.gravityScale;

    }


    void Update()
    {
        if (inputSystem.Player.Jump.phase == InputActionPhase.Waiting)
        {
            //Debug.Log(inputSystem.Player.Jump.phase == InputActionPhase.Waiting);
            jumpButtonReleased = true;
        }

        if (Mathf.Abs(transform.position.x) > xRange)  // Constrain for xRange (Boundary)
        {
            transform.position = new Vector3(Mathf.Sign(transform.position.x) * xRange, transform.position.y, transform.position.z);
        }

        if (IsGrounded() && playerRb.velocity.y <= 0.1f)   //Checks if player is on the ground and not jumping
        {
            jumpCount = 0;
            anim.SetBool("is_jumping", false);
        }

    }

    private void FixedUpdate()
    {
        horizontalMovement();        //logic for horizontal movement

        RunAnimation();              //set animation for running
        
        BufferJump();                //add a buffer jump
        
        FlipPlayer();                //flip sprite based on its direction
        
        AffectGravity();             //Adds Max Fall Speed and faster falling

    }

    //Buffer time which enables player to jump even if he presses jump before he lands making it a bit responsive
    private void BufferJump()
    {
        if (jumpBuffered)
        {
            jumpBufferCounter += Time.deltaTime;
            if (jumpBufferCounter >= jumpBufferTimer)
            {
                jumpBuffered = false; // Reset the buffer if time has expired
            }
            else if (IsGrounded()) // Execute the jump if grounded and buffered
            {
                onJump();
                jumpCount = 1;
                jumpBuffered = false;
            }
        }
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        Debug.Log(obj);
        if (obj.phase == InputActionPhase.Started && jumpButtonReleased)
        {
            jumpButtonReleased = false;
            if (jumpCount >= maxJump)     //
            {
                jumpBuffered = true;
                jumpBufferCounter = 0;

            }
            else
            {
                onJump();
                jumpCount++;
                //jumpBuffered = true;

            }
        }

    }

    public void onDamage()
    {
        playerHealth -= 1;
        if (playerHealth <= 0)
        {
            Debug.Log("You Died");
            return;
        }
        Debug.Log("Life Remaining: " + playerHealth);
    }

    public void pogoMovement()
    {
        onJump(power: pogoJumpPower);
    }

    private void RunAnimation()
    {
        if (Mathf.Abs(playerRb.velocity.x) > 0)
        {
            anim.SetFloat("speed", 0.6f);
        }
        else
        {
            anim.SetFloat("speed", 0.3f);
        }
    }

    // horizontal Movement
    private void horizontalMovement()
    {
        horizontalInput = inputSystem.Player.Movement.ReadValue<float>();
        playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);
    }

    private void FlipPlayer()
    {
        if (horizontalInput > 0.1f)
            spriteRenderer.flipX = false;
        else if (horizontalInput < -0.1)
            spriteRenderer.flipX = true;
    }

    private void AffectGravity()
    {
        if (playerRb.velocity.y < -maxVelocityY)        //Max Fall Speed
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -maxVelocityY);
        }
        else if (playerRb.velocity.y < 0)               // Fast Falling
        {
            playerRb.gravityScale = fallingGravity;
        }
        else                                            // Set Gravity normal again once landed
        {
            playerRb.gravityScale = initialGravity;
        }
    }

    private void onJump(float power = jumpPower)
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, power);
        anim.SetBool("is_jumping", true);
    }

    //checks if player is touching the ground
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit2D.collider != null;
    }

}
