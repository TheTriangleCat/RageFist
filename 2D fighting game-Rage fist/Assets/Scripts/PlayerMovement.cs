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
    private PlayerControls playerControls;

    [Header("Walking")]
    private Vector2 moveDirection;

    public float startFriction;
    public float endFriction;

    public Rigidbody2D playerRigidbody;
    public float playerSpeed;

    //[Header("Jumping")]
    

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
        playerControls = new PlayerControls();

        playerControls.Player.Move.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerControls.Player.Move.canceled += ctx => moveDirection = Vector2.zero;
    }

    private void Start()
    {
        playerRigidbody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Functions
    private void MovePlayer()
    {
        Vector2 velocity = playerRigidbody.velocity;

        plrVelocity.text = "Player Velocity: " + velocity.ToString();

        // Movement
        // Flipping the player
        if (moveDirection.x == 1f)
        {
            playerRigidbody.velocity += new Vector2(startFriction, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        else if (moveDirection.x == -1f) 
        {
            playerRigidbody.velocity -= new Vector2(startFriction, 0f);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        // End friction for left side of player
        if (transform.rotation.eulerAngles.y == 180f && moveDirection.x == 0f)
        {
            playerRigidbody.velocity += new Vector2(endFriction, 0f);

            if (playerRigidbody.velocityX >= 0f) 
            {
                playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity, 0f);
            }
        }

        // End friction for right side of player
        if (transform.rotation.eulerAngles.y == 0f && moveDirection.x == 0f)
        {
            playerRigidbody.velocity -= new Vector2(endFriction, 0f);

            if (playerRigidbody.velocityX <= 0f)
            {
                playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity, 0f);
            }
        }

        // Limit the velocity
        Vector2 flatVel = new Vector2(playerRigidbody.velocity.x, 0f);

        if (flatVel.magnitude > playerSpeed)
        {
            Vector2 limitedVel = flatVel.normalized * playerSpeed;
            playerRigidbody.velocity = new Vector2(limitedVel.x, 0f);
        }

        // Jumping (Testing phase)
        if (Input.GetKey (KeyCode.Space)) 
        {
            playerRigidbody.AddForceY(50f);
        }
    }
}
