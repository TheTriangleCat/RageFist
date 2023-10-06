using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    [Header("Controls")]
    private PlayerInput playerControls;

    [Header("Walking")]
    public float speed = 1f;
    [SerializeField] float speedDivident = 100f;

    public Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    public bool isGrounded;
    public LayerMask anythingMask;

    [Header("Jumping")]
    public float yVelocity = 0.0001f;
    [SerializeField] float currentYVel;

    public float jumpYVel = 0.01f;
    public float jumpYVelStartup;

    //Controls (Awake gets called before Start)
    private void Awake()
    {
        playerControls = new PlayerInput();

        //playerControls.Player.Move.performed += 
    }

    private void Start()
    {
        jumpYVelStartup = groundDistance;
    }

    private void Update()
    {
        IsGrounded();
        MovementRightLeft();
        Jump();
        Fall();
    }

    // Functions
    void MovementRightLeft()
    {


        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed / speedDivident, 0f, 0f);
            Debug.Log("going right");
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed / speedDivident, 0f, 0f);
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
            currentYVel -= yVelocity;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            //transform.Translate(0,0,0);
            //currentYVel = jumpYVel;
        }
    }

    private void IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, anythingMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
