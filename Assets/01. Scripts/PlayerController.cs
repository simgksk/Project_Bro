using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Setting")]
    public float moveSpeed = 5f;
    public float moveDistance = 0.5f;
    
    [Header("Jump Setting")]
    public float jumpForce = 10f;
    public float forwardForce = 5f;

    [Header("Ground Check")]
    public LayerMask gruondLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    Rigidbody rb;
    bool isGrounded;
    bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GroundCheck();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            JumpForward();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRightOnce();
        }

        if(!isGrounded && isJumping)
        {
            Vector3 vel = rb.velocity;
            vel.x = 0f;
            vel.y = 0f;
            rb.velocity = vel;
        }
    }

    private void MoveRightOnce()
    {
        transform.position += Vector3.right * moveSpeed * moveDistance;
    }

    private void JumpForward()
    {
        rb.velocity = Vector3.zero;

        Vector3 jumpDirection = transform.right * forwardForce + Vector3.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode.Impulse);

        isJumping = true;
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, gruondLayer);

        if (isGrounded)
        {
            isJumping = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
