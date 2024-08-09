using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [SerializeField] float pogoJumpPower;
    public static float playerHealth;


    private Rigidbody2D playerRb;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

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


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerHealth = 3;

        jumpCount = 0;
        initialGravity = playerRb.gravityScale;

    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x) > xRange)  // Constrain for xRange
        {
            transform.position = new Vector3(Mathf.Sign(transform.position.x) * xRange, transform.position.y, transform.position.z);
        }

        if (jumpCount >= maxJump)
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
                    Jump();
                    jumpCount = 1;
                    jumpBuffered = false;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                jumpCount++;
                //jumpBuffered = true;
            }
        }



        if (Input.GetKeyDown(KeyCode.Space) && jumpCount >= maxJump)
        {
            jumpBuffered = true;
            jumpBufferCounter = 0;
        }

        if (IsGrounded() && playerRb.velocity.y <= 0.1f)   //Checks if player is on the ground and not jumping
        {
            jumpCount = 0;
            anim.SetBool("is_jumping", false);
        }

        //Debug.Log("Jump Count : " + jumpCount);
        //trail Renderer
        //isMoving = playerRb.velocity != Vector2.zero;
        //trailRenderer.enabled = isMoving;
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
        playerRb.velocity = new Vector2(playerRb.velocity.x, pogoJumpPower);
        anim.SetBool("is_jumping", true);
    }
    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y); // horizontal Movement
        //Debug.Log(Mathf.Abs(playerRb.velocity.x));
        if (Mathf.Abs(playerRb.velocity.x) > 0)
        {
            anim.SetFloat("speed", 0.6f);
        }
        else
        {
            anim.SetFloat("speed", 0.3f);
        }

        FlipPlayer();               //flip sprite based on its direction
        AffectGravity();             //Adds Max Fall Speed and faster falling

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
    private void Jump()
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, jumpPower);
        anim.SetBool("is_jumping", true);

    }
    //checks if player is touching the ground
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit2D.collider != null;
    }

}
