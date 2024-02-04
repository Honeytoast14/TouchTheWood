using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class IsometricPlayerMovementController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 moveDirection;
    public float moveSpeed = 5f;

    public Animator animator;

    float horizontalInput;
    float verticalInput;
    float movementSpeed;

    public bool canMove = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        Animate();
    }

    public void Move()
    {
        if (canMove)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            rb.velocity = new Vector2(horizontalInput, verticalInput) * moveSpeed;

            moveDirection = new Vector2(horizontalInput, verticalInput);
            movementSpeed = Mathf.Clamp(moveDirection.magnitude, 0.0f, 1.0f);
        }
    }

    void Animate()
    {
        if (moveDirection != Vector2.zero)
        {
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
        }

        animator.SetFloat("Speed", movementSpeed);
    }
}