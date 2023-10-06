using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    [Header("Testing stuff (deleat after)")]
    public TextMeshProUGUI plrVelocity;

    [Header("Controls")]
    private PlayerInput playerControls;

    [Header("Walking")]
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector3 velocity = rb.velocity;

        plrVelocity.text = "Player Velocity: " + velocity.ToString();

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(playerSpeed / 100, 0f, 0f); // Divided by 100 so that we can keep the value of speed small
            transform.rotation = Quaternion.Euler(0, 0, 0); // Flip player
            Debug.Log("going right");
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(playerSpeed / 100, 0f, 0f); // Divided by 100 so that we can keep the value of speed small
            transform.rotation = Quaternion.Euler(0, 180, 0); // Flip player
            Debug.Log("going left");
        }
    }

    private void Fall()
    {
        transform.Translate(0, currentYVel, 0);

        if (isGrounded) 
        {
            currentYVel = 0;
        }

        else
        {
            currentYVel -= gravity;
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
