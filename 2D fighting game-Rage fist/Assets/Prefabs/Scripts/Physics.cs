using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Physics : MonoBehaviour
{
    [Header("Testing stuff (deleat after)")]
    public TextMeshProUGUI plrVelocity;

    [Header("Controls")]
    //private PlayerInput playerControls;
    public InputAction playerControls;

    [Header("Walking")]
    public Rigidbody2D playerRigidbody;
    public float playerSpeed;
    //[SerializeField] float speedDivident = 100f;

    public Transform groundCheck;

    [SerializeField] float groundDistance;

    public bool isGrounded;
    public LayerMask anythingMask;

    [Header("Jumping")]
    public float gravity;

    [SerializeField] float currentYVel;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    //Controls (Awake gets called before Start)
    private void Awake()
    {
       // playerControls = new PlayerInput();

        //playerControls.Player.Move.performed += 
    }

    private void Start()
    {
        playerRigidbody.freezeRotation = true;
        //gravity /= 10000f;
    }

    private void FixedUpdate()
    {
        IsGrounded();
        MovementRightLeft();
        Jump();
        Fall();
    }

    // Functions
    private void MovementRightLeft()
    {
        Vector3 velocity = playerRigidbody.velocity;

        plrVelocity.text = "Player Velocity: " + velocity.ToString();

        // Movement
        Vector2 moveDirection = playerControls.ReadValue<Vector2>();
        Debug.Log(moveDirection);
        playerRigidbody.velocity = new Vector2(moveDirection.x * playerSpeed, playerRigidbody.velocity.y);
//        transform.rotation = Quaternion.Euler(0f, moveDirection.y * 180f, 0f); // Flip player

        if (Input.GetKey (KeyCode.Space)) 
        {
            playerRigidbody.AddForceY(50);
        }
    }

    private void Fall()
    {
       // transform.Translate(0, currentYVel, 0);

        if (isGrounded) 
        {
            //currentYVel = 0;
        }

        else
        {
            //currentYVel -= gravity;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {

        }
    }

    private void IsGrounded()
    {
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, anythingMask);
    }

    // Draws the circle visual for floor collision
   // private void OnDrawGizmos()
    //{
      //  Gizmos.color = Color.yellow;
       // Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
   // }
}
