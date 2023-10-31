using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerMovement : MonoBehaviour
{
    [Header("Testing stuff (deleat after)")]
    public TextMeshProUGUI plrVelocity;

    [Header("Controls")]
    private PlayerControls playerControls;

    [Header("Walking")]
    public Rigidbody2D playerRigidbody;
    private Vector2 playerVelocity;

    private Vector2 moveDirection;

    public float startFriction;
    public float endFriction;

    public float playerSpeed;

    [Header("Jumping")]
    public LayerMask groundLayer;

    public Transform groundCheck;
    public BoxCollider2D jumpDetect;

    public float circleSize; // We make a circle and then the circle checks if its touching the floor

    [SerializeField] private bool onGround;
    [SerializeField] private bool canJump;

    private float defaultGravityScale;

    public float gravityFalloff; // Gravity scale after reaching max height
    public float maxGravityFalloff; // Max gravity fallof
    public float jumpPower;

    [SerializeField] InputAction buttonAction;

    //Controls 
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Awake() // Awake gets called before Start
    {
        playerControls = new PlayerControls();

        playerControls.Player.PlayerControls.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerControls.Player.PlayerControls.canceled += ctx => moveDirection = Vector2.zero;

        if (buttonAction != null)
        {
            buttonAction.Enable();
        }
        else
        {
            Debug.LogError("buttonAction is not assigned in the Inspector.");
        }
    }

    // Movement system
    private void Start()
    {
        defaultGravityScale = playerRigidbody.gravityScale;

        playerRigidbody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Jump();

        StartCoroutine(CheckGround());
    }

    // Functions
    private void MovePlayer()
    {
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

    private void Jump()
    {
        if(onGround)
        {
            canJump = true;
        }
        if (onGround && canJump)//&& canJump
        {
            playerRigidbody.gravityScale = defaultGravityScale;

            if (moveDirection.y == 1f) 
            {
                playerRigidbody.velocityY = jumpPower;
            }
        }

        if (buttonAction.triggered&&canJump)
        {
            canJump = false;
            Debug.Log("Button was single-tapped in the Update method.");
            playerRigidbody.gravityScale = defaultGravityScale;
            playerRigidbody.velocityY = jumpPower * 1.5f;
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
    }

    // Fix double jump when going to another floor
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMask.LayerToName(groundLayer)) - 3 && Physics2D.IsTouching(collision.gameObject.GetComponent<BoxCollider2D>(), jumpDetect))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMask.LayerToName(groundLayer)) - 3 && Physics2D.IsTouching(collision.gameObject.GetComponent<BoxCollider2D>(), jumpDetect))
        {
            canJump = false;
        }
    }

    // Check ground coroutine
    IEnumerator CheckGround()
    {
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
