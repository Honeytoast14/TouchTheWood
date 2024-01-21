using DS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 moveDirection;
    public float moveSpeed = 5f;

    public bool canTalk = false;
    public bool checkSide = false;

    public Animator animator;

    float horizontalInput;
    float verticalInput;
    float movementSpeed;

    TriggerEvent triggerTalk;
    void Start()
    {
        triggerTalk = FindObjectOfType<TriggerEvent>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

   void Update()
    {
        Move();
        Animate();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(canTalk == true)
            {
                triggerTalk.StartDialogue();
                //Debug.Log("Is talking");
            }

            if(checkSide == true)
            {
                Debug.Log("This is " + " side");
            }
        }
    }

    public void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(horizontalInput, verticalInput) * moveSpeed;

        moveDirection = new Vector2(horizontalInput, verticalInput);
        movementSpeed = Mathf.Clamp(moveDirection.magnitude, 0.0f, 1.0f);
    }

    void Animate()
    {
        if (moveDirection != Vector2.zero) {
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
        }

        animator.SetFloat("Speed", movementSpeed);
    }
}