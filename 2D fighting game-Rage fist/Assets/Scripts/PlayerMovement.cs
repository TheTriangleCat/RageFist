using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Windows;
using static UnityEditor.Timeline.TimelinePlaybackControls;

/// <summary>
///  Documentation on the input system: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Interactions.html
///  Useful tutorials: https://www.youtube.com/watch?v=ZSP3bFaZm-o

///  Unity's new input system has multiple control type.
///  There is the vector2, wich will return a vector2 value depending on what keys you press.
///  Ex.: if I press the A key, it will return -1 on the x axis.
///  The other control type we will use is the buttons. These doesn't return anything. We only detect if its pressed.
///  With the button control type, we can detect if its beeing held or tapped. We get more flexibility.

/// In this case, we are going to use the button control type and we are setting it manualy to give us more flexibility and control over the controls.
/// It is useful since we are going to have multiple controls for multiple players so we can check wether its player1 or player2 that does the controls.
/// </summary>

public class Physics : MonoBehaviour
{
    [Header("Testing stuff (deleat after)")]
    public TextMeshProUGUI plrVelocity;

    [Header("Controls")]
    private PlayerControls playerControls; // This is automatically adding inputs, no need to subscribe and unsubscribe

    private InputAction walk;

    [Header("Walking")]
    public Rigidbody2D playerRigidbody;
    private Vector2 playerVelocity;

    private Vector2 moveDirection; // X axis for moving

    public float startFriction;
    public float endFriction;

    public float playerSpeed;

    [Header("Jumping")]
    public LayerMask groundLayer;

    public Transform groundCheck;
    public BoxCollider2D jumpDetectction;

    public float circleSize; // We make a circle and then the circle checks if its touching the floor

    private bool onGround;
    private bool canJump;
    public bool canDoubleJump; // This setting is to check if the player can double jump or not

    private bool jumpKeyHeld;
    private bool readyToJump; // This is to check if the player has landed and is ready to jump so it can't be spammed if the player holds key down

    private float defaultGravityScale;
    
    public float gravityFalloff; // Gravity scale after reaching max height
    public float maxGravityFalloff; // Max gravity fallof

    public float jumpCooldown;
    public float jumpPower;

    // Input system
    private void Awake()
    {
        playerControls = new PlayerControls();

        // For walking only
        playerControls.Player.Walking.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerControls.Player.Walking.canceled += ctx => moveDirection = Vector2.zero;
        playerControls.Player.Walking.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerControls.Player.Walking.canceled += ctx => moveDirection = Vector2.zero;
    }

    // Movement system
    private void Start()
    {
        resetReadyToJump();

        defaultGravityScale = playerRigidbody.gravityScale;

        playerRigidbody.freezeRotation = true;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Movement system
    private void FixedUpdate()
    {
        // Adding force to jumping
        if (jumpKeyHeld && readyToJump && onGround)
        {
            canJump = false;
            readyToJump = false;

            playerRigidbody.gravityScale = defaultGravityScale;
            playerRigidbody.velocityY = jumpPower;

            Invoke(nameof(resetReadyToJump), jumpCooldown);
        }

        // Increasing gravity when the player is falling
        if (Mathf.Abs(playerRigidbody.velocityY) >= jumpPower)
        {
            playerRigidbody.gravityScale += gravityFalloff;
        }

        // Clamping the max gravity scale
        if (playerRigidbody.gravityScale > maxGravityFalloff)
        {
            playerRigidbody.gravityScale = maxGravityFalloff;
        }

        // Coroutine for onGround bool so it runs at the same time
        StartCoroutine(CheckGround());

        // Moving the player (inputs are done manually here)
        MovePlayer();
    }

    // Functions
    private void MovePlayer()
    {
        Debug.Log(moveDirection);
        playerVelocity = playerRigidbody.velocity;

        plrVelocity.text = "Player Velocity: " + playerVelocity.ToString();

        // Movement
        // Flipping the player
        if (moveDirection.x == 1f)
        {
            playerRigidbody.velocityX += startFriction;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        else if (moveDirection.x == -1f)
        {
            playerRigidbody.velocityX -= startFriction;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        // End friction for left side of player
        if (transform.rotation.eulerAngles.y == 180f && moveDirection.x == 0f)
        {
            playerRigidbody.velocityX += endFriction;

            if (playerRigidbody.velocityX >= 0f) 
            {
                playerRigidbody.velocityX = 0f;
            }
        }

        // End friction for right side of player
        if (transform.rotation.eulerAngles.y == 0f && moveDirection.x == 0f)
        {
            playerRigidbody.velocityX -= endFriction;

            if (playerRigidbody.velocityX <= 0f)
            {
                playerRigidbody.velocityX = 0f;
            }
        }

        // Limit the walking velocity
        Vector2 flatVel = new Vector2(playerRigidbody.velocityX, 0f);

        if (flatVel.magnitude > playerSpeed)
        {
            Vector2 limitedVel = flatVel.normalized * playerSpeed;
            playerRigidbody.velocityX = limitedVel.x;
        }
    }

    public void Jump(InputAction.CallbackContext context) // We add force to jump separatly because the input system doesn't have a key held down so we get crafty here and do it in FixedUpdate
    {
        if (context.performed)
        {
            jumpKeyHeld = true;
        }

        else if (context.canceled)
        {
            jumpKeyHeld = false;
        }
    }

    private void resetReadyToJump()
    { 
        readyToJump = transform;
    }

    // Fix double jump when going to another floor
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && Physics2D.IsTouching(collision.gameObject.GetComponent<BoxCollider2D>(), jumpDetectction))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && Physics2D.IsTouching(collision.gameObject.GetComponent<BoxCollider2D>(), jumpDetectction))
        {
            canJump = false;
        }
    }

    // Check ground coroutine
    IEnumerator CheckGround()
    {
        if (onGround)
        {
            canDoubleJump = true;
            readyToJump = true;
        }

        if (canJump)
        {
            onGround = Physics2D.OverlapCircle(groundCheck.position, circleSize, groundLayer);
        }

        else
        {
            onGround = false;
        }

        yield return canJump;
    }

    //Draw the circle preview
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, circleSize);
    }
}
