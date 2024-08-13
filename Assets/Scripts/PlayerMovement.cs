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

    public float playerHealth { get; private set; }
    public bool isPlayerHit { get; set; }


    private Rigidbody2D playerRb;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private InputSystem inputSystem;

    private Vector2 input;

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
    private float jumpBufferCounter;
    private float dashPower = 24f;
    private float dashTime = 0.2f;
    private float dashCooldownTime = 1f;
    private float invincibilityAfterHit = 0.8f;
    private bool jumpButtonReleased = true;
    private bool isDashing;
    private bool canDash;
    private float timer;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        inputSystem = new InputSystem();

        playerHealth = 3;
        jumpCount = 0;
        timer = 0;
        initialGravity = playerRb.gravityScale;
        canDash = true;
        isDashing = false;
        isPlayerHit = false;
    }

    private void OnEnable()
    {
        inputSystem.Player.Enable();
        inputSystem.Player.Jump.started += Jump;
        inputSystem.Player.Dash.started += Dash_Started;
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
    }

    private void Dash_Started(InputAction.CallbackContext obj)
    {
        if (canDash && input != Vector2.zero)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float orignalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0;
        playerRb.velocity = input.normalized * dashPower;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        playerRb.gravityScale = orignalGravity;
        playerRb.velocity = Vector3.zero;
        yield return new WaitForSeconds(dashCooldownTime);
        canDash = true;
    }

    void Update()
    {
        if (isPlayerHit)
        {
            timer += Time.deltaTime;
            if (timer > invincibilityAfterHit)
            {
                isPlayerHit = false;
                timer = 0;
            }

        }
        if (inputSystem.Player.Jump.phase == InputActionPhase.Waiting)
        {
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

        FlipPlayer();                //flip sprite based on its direction

        gameObject.GetComponent<BoxCollider2D>().enabled = !isDashing;
        transform.GetChild(1).gameObject.GetComponent<BoxCollider2D>().enabled = !isDashing;

    }

    private void FixedUpdate()
    {
        if (isDashing) return;       //dont do anything if the player is dashing

        horizontalMovement();        //logic for horizontal movement

        RunAnimation();              //set animation for running

        BufferJump();                //add a buffer jump

        AffectGravity();             //Adds Max Fall Speed and faster falling

    }

    // horizontal Movement
    private void horizontalMovement()
    {
        input = inputSystem.Player.Movement.ReadValue<Vector2>();
        playerRb.velocity = new Vector2(input.x * speed, playerRb.velocity.y);
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

    private void Jump(InputAction.CallbackContext obj)
    {
        if (isDashing) return;
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

    private void onJump(float power = jumpPower)
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, power);
        anim.SetBool("is_jumping", true);
    }

    public void onDamage()
    {
        if (!isDashing)
        {
            isPlayerHit = true;
            playerHealth -= 1;
            anim.SetTrigger("player_hit");
        }
    }

    public void pogoMovement()
    {
        if (!isDashing)
        {
            onJump(power: pogoJumpPower);
        }
    }



    private void FlipPlayer()
    {
        if (input.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (input.x < -0.1)
            spriteRenderer.flipX = true;
    }


    //checks if player is touching the ground
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit2D.collider != null;
    }

}
