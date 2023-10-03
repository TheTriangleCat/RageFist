using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class movement : MonoBehaviour
{

    public float speed = 1f;
    [SerializeField] float speedDivident = 100f;

    public Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    public bool isGrounded;
    public LayerMask anythingMask;

    public float yVelocity = 0.0001f;
    [SerializeField] float currentYVel;

    public float jumpYVel = 0.01f;
    public float jumpYVelStartup;


    void Start()
    {
        jumpYVelStartup = groundDistance;
    }

    
    void MovementRightLeft()
    {
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed/speedDivident, 0f, 0f);
            Debug.Log("going right");
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed/speedDivident, 0f, 0f);
            Debug.Log("going left");
        }
    }

    void Update()
    {
        IsGrounded();
        MovementRightLeft();
        Jump();
        Fall();
    }

    void Fall()
    {
        transform.Translate(0,currentYVel,0);

        if(!isGrounded)
        {
            currentYVel = currentYVel - yVelocity;
        }
        else
        {
            currentYVel = 0;
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            transform.Translate(0,0,0);
            currentYVel = jumpYVel;
        }
    }

    void IsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, anythingMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
