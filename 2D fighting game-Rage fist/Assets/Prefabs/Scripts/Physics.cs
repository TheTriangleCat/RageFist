using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Physics : MonoBehaviour
{
    [Header("Testing stuff (deleat after)")]
    public TextMeshProUGUI plrVelocity;

    [Header("Controls")]
    private PlayerInput playerControls;

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

    private void Update()
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

        if (Input.GetKey(KeyCode.D))
        {
            playerRigidbody.AddForceX(playerSpeed * 10 * Time.deltaTime);

            Vector3 flatVel = new Vector2(playerRigidbody.velocity.x, 0f);

            // limit velocity if needed
            if (flatVel.magnitude > playerSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * playerSpeed * Time.deltaTime;
                playerRigidbody.velocity = new Vector3(limitedVel.x, playerRigidbody.velocity.y, limitedVel.z);
            }

            transform.rotation = Quaternion.Euler(0, 0, 0); // Flip player
            Debug.Log("going right");
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerRigidbody.AddForceX(-playerSpeed * 10 * Time.deltaTime);

            Vector3 flatVel = new Vector2(playerRigidbody.velocity.x, 0f);

            // limit velocity if needed
            if (flatVel.magnitude > playerSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * playerSpeed * Time.deltaTime;
                playerRigidbody.velocity = new Vector3(limitedVel.x, playerRigidbody.velocity.y, limitedVel.z);
            }

            transform.rotation = Quaternion.Euler(0, 180, 0); // Flip player
            Debug.Log("going left");
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, anythingMask);
    }

    // Draws the circle visual for floor collision
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
