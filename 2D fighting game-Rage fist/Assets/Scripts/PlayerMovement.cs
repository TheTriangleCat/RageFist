using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class Physics : MonoBehaviour
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
    public Transform jumpCheck;

    public float circleSize; // We make a circle and then the circle checks if its touching the floor
    public float circleSize2;

    private bool onGround;

    private float defaultGravityScale;

    public float gravityFalloff; // Gravity scale after reaching max height
    public float jumpPower;
    [SerializeField] bool doubleJump;
    [SerializeField] bool doubleJumpCheck;

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

        CheckGround();
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

        if (onGround)
        {
            doubleJumpCheck = true;
            playerRigidbody.gravityScale = defaultGravityScale;

            if (moveDirection.y == 1f) 
            {
                playerRigidbody.velocityY = jumpPower;
            }
        }
        else if (doubleJump = !Physics2D.OverlapCircle(jumpCheck.position, circleSize2, groundLayer) && doubleJumpCheck)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRigidbody.gravityScale = defaultGravityScale;
                playerRigidbody.velocityY = jumpPower;
                doubleJumpCheck = false;
            }

        }

        // Increasing gravity when the player is falling
        if (Mathf.Abs(playerRigidbody.velocityY) >= jumpPower)
        {
            playerRigidbody.gravityScale += gravityFalloff;
        }
    }

    private void CheckGround()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, circleSize, groundLayer);
    }

    //Draw the circle preview
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, circleSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(jumpCheck.position, circleSize2);
    }
}
