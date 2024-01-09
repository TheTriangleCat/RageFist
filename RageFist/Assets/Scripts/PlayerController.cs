using System;
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

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Testing stuff (deleat after)")]
    [SerializeField] private TextMeshProUGUI plrVelocity;

    [Header("Controls")]
    private PlayerControls playerControls; // This is automatically adding inputs, no need to subscribe and unsubscribe

    [Header("Walking")]
    [SerializeField] private bool canMove; // Determines if the player can move, can be changed through other code

    [SerializeField] private GameObject playerModel;
    [SerializeField] private Rigidbody2D playerRigidbody;

    private Vector2 playerVelocity;
    private Vector2 moveDirection; // X axis for moving

    [SerializeField] private float startFriction;
    [SerializeField] private float endFriction;

    [SerializeField] private float playerSpeed;

    [Header("Jumping")]
    private GameObject groundTag;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private Vector2 boxSize; // We make a box and then the box checks if its touching the floor

    private bool onGround;

    private bool jumpKeyTapped; // For double jump 
   
    private bool canJump; // This is to check if the player has landed and is ready to jump so it can't be spammed if the player holds key down
    
    [SerializeField] private float jumpCooldown; // Set to 0 if you don't want any cooldown

    [SerializeField] private bool canDoubleJump; // Checks wether if the player can double jump or no

    [SerializeField] private float jumpPower;
    [SerializeField] private ParticleSystem jumpingParticle;

    [SerializeField] private float defaultGravityScale;

    [SerializeField] private float gravityFalloff; // Gravity scale after reaching max height
    [SerializeField] private float maxGravityFalloff; // Max gravity fallof

    [SerializeField] GameObject particleSystemJump;
    #endregion

    //Draw the circle preview for onGround  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Jumping.performed += JumpInput;

        playerControls.Player.Walking.performed += context => moveDirection = context.ReadValue<Vector2>();
        playerControls.Player.Walking.canceled += context => moveDirection = Vector2.zero;
    }
    
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        ResetCanJump();

        playerRigidbody.gravityScale = defaultGravityScale;
        playerRigidbody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            StartCoroutine(CheckGround());
            MovePlayer();
        }
    }

    #region Functions
    private void MovePlayer()
    {
        plrVelocity.text = "Player Velocity: " + playerVelocity.ToString();

        // Movement and flipping player
        int defaultLocalScale = 1;

        playerRigidbody.velocity = new Vector2(playerVelocity.x, playerRigidbody.velocity.y); // This is to apply the velocity to the rigidbody, we only set the variable of the velocity, not the actual velocity
        playerVelocity.x += startFriction * moveDirection.x;

        if (moveDirection.x != 0f)
            transform.localScale = new(moveDirection.x, defaultLocalScale, defaultLocalScale);

        // End friction 
        if (moveDirection.x == 0f)
        {
            playerVelocity.x += -endFriction * transform.localScale.x;

            if (transform.localScale.x == defaultLocalScale && playerVelocity.x < 0f)
                playerVelocity.x = 0f;

            else if (transform.localScale.x == -defaultLocalScale && playerVelocity.x > 0f)
                playerVelocity.x = 0f;
        }

        // Limit the walking velocity
        Vector2 flatVel = new(playerVelocity.x, 0f);

        if (flatVel.magnitude > playerSpeed)
        {
            Vector2 limitedVel = flatVel.normalized * playerSpeed;
            playerVelocity.x = limitedVel.x;
        }
    }

    #region Jumping functions
    private void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed && canJump && onGround)
        {
            Jump();
        }

        if (context.performed && !onGround && jumpKeyTapped && canDoubleJump) // small bug where the player can dbj if it hits a platform while in the air pls fixxx
        {
            jumpKeyTapped = false;
            Jump();
        }
    }

    private void Jump()
    {
        canJump = false;

        // Adding force to jumping
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpPower);

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

    private void ResetCanJump()
    {
        canJump = true;
        jumpKeyTapped = true;
    }
    #endregion
    #endregion

    private IEnumerator CheckGround() // This is a coroutine so it runs at the same time as the fixed update
    {
        onGround = Physics2D.OverlapBox(groundCheck.position, boxSize, 0f) && playerRigidbody.velocity.y == 0f;

        // Ground detection
        if (onGround)
        {
            playerRigidbody.gravityScale = defaultGravityScale;
            
            ResetCanJump();
            Invoke(nameof(ResetCanJump), jumpCooldown);
        }

        yield return onGround;
    }
}