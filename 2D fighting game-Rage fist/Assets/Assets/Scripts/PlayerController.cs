using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * This code is to controll the player, we call functions from other scripts to keep everything organized.  
 * This script is the brain of the controls for the player
*/

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

/// For best results in double jump, respect the ratio of 0.05 JumpCooldown:1 JumpPower
/// </summary>

public class PlayerController : MonoBehaviour
{
    public bool canMove; // Determines if the player can move, can be changed through other code

    #region Player Movement
    [Header("Testing stuff (deleat after)")]
    public TextMeshProUGUI plrVelocity;

    [Header("Controls")]
    private PlayerControls playerControls; // This is automatically adding inputs, no need to subscribe and unsubscribe

    [Header("Walking")]
    public Rigidbody2D playerRigidbody;
    private Vector2 playerVelocity;

    private Vector2 moveDirection; // X axis for moving

    public float startFriction;
    public float endFriction;

    public float playerSpeed;
    #endregion

    #region Jumping
    [Header("Jumping")]

    public LayerMask groundLayer;
    public Transform groundCheck;

    public Vector2 boxSize; // We make a box and then the box checks if its touching the floor

    private bool onGround;

    private bool jumpKeyHeld;
    private bool canJump; // This is to check if the player has landed and is ready to jump so it can't be spammed if the player holds key down
    public bool canDoubleJump; // Checks wether if the player can double jump or no
    public float jumpCooldown; // Set to 0 if you don't want any cooldown
    public float jumpPower;
    public ParticleSystem jumpingParticle;

    private float defaultGravityScale;

    public float gravityFalloff; // Gravity scale after reaching max height
    public float maxGravityFalloff; // Max gravity fallof

    [SerializeField] GameObject particleSystemJump;
    #endregion

    // Input system
    #region Implementing the new input system, we have to do the walking manually by subscribing to it. The jumping is done automatically without subscribing.
    private void Awake()
    {
        playerControls = new PlayerControls();

        // For walking only
        playerControls.Player.Walking.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerControls.Player.Walking.canceled += ctx => moveDirection = Vector2.zero;
        playerControls.Player.Walking.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerControls.Player.Walking.canceled += ctx => moveDirection = Vector2.zero;
    }
    
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion

    // Movement system
    #region The movement system. Has walking, jumping, and double jump.
    private void Start()
    {
        canJump = true;

        defaultGravityScale = playerRigidbody.gravityScale;

        playerRigidbody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            // Coroutine for onGround bool so it runs at the same time
            StartCoroutine(CheckGround());

            // Moving the player (inputs are done manually here)
            MovePlayer();

            // Jumping
            if (onGround && jumpKeyHeld && canJump)
            {
                canJump = false;
                Jump();

                // Double jump
                if (canDoubleJump)
                {
                    Debug.Log("Dkddkdkk");
                    Jump();
                }
            }
        }   

        // Applying the velocity to rigidbody (we only set the variable "playerVelocity", not the actual velocity)
        playerRigidbody.velocity = playerVelocity;
    }
    #endregion
    
    // Functions
    #region Functions for the movement, the jumping one is detecting if the player is holding it. We adding the jumping force at FixedUpdate.
    // Walking
    private void MovePlayer()
    {
        playerVelocity = playerRigidbody.velocity;

        plrVelocity.text = "Player Velocity: " + playerVelocity.ToString();

        // Movement
        // Flipping the player
        if (moveDirection.x == 1f)
        {
            playerVelocity.x += startFriction;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        else if (moveDirection.x == -1f)
        {
            playerVelocity.x -= startFriction;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        // End friction for left side of player
        if (transform.rotation.eulerAngles.y == 180f && moveDirection.x == 0f)
        {
            playerVelocity.x += endFriction;

            if (playerVelocity.x >= 0f)
            {
                playerVelocity.x = 0f;
            }
        }

        // End friction for right side of player
        if (transform.rotation.eulerAngles.y == 0f && moveDirection.x == 0f)
        {
            playerVelocity.x -= endFriction;

            if (playerVelocity.x <= 0f)
            {
                playerVelocity.x = 0f;
            }
        }

        // Limit the walking velocity
        Vector2 flatVel = new Vector2(playerVelocity.x, 0f);

        if (flatVel.magnitude > playerSpeed)
        {
            Vector2 limitedVel = flatVel.normalized * playerSpeed;
            playerVelocity.x = limitedVel.x;
        }
    }

    // Jumping
    #region Jumping (3 functions because the SingleJumpInput and other one is the one detecting input from the new input system and we check if the player can jump then we acll the function)
    private void Jump()
    {
        // Adding force to jumping
        playerVelocity.y = jumpPower;

        particleSystemJump.GetComponent<ParticleSystem>().Play(true);

        // Increasing gravity when the player is falling
        if (Mathf.Abs(playerVelocity.y) >= jumpPower)
        {
            playerRigidbody.gravityScale += gravityFalloff;
        }

        // Clamping the max gravity scale
        if (playerRigidbody.gravityScale > maxGravityFalloff)
        {
            playerRigidbody.gravityScale = maxGravityFalloff;
        }
    }
    
    public void JumpInput(InputAction.CallbackContext context) // We add force to jump separatly because the input system doesn't have a key held down so we get crafty here and do it in FixedUpdate
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
    #endregion

    // Check ground coroutine
    IEnumerator CheckGround()
    {
        onGround = Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);

        // Setting some stuff for double jump
        if (onGround)
        {
            playerRigidbody.gravityScale = defaultGravityScale;
            canJump = true;
        }

        yield return onGround;
    }
    #endregion

    //Draw the circle preview for onGround  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }
}