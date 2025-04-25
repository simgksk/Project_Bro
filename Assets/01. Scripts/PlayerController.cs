using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Setting")]
    public float jumpForce = 6f;
    public float forwardForce = 3f;

    [Header("Ground Check")]
    public LayerMask gruondLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    Rigidbody rb;
    bool isGrounded;

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
    }

    private void JumpForward()
    {
        rb.velocity = Vector3.zero;

        Vector3 jumpDirection = transform.right * forwardForce + Vector3.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode.Impulse);
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, gruondLayer);
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
