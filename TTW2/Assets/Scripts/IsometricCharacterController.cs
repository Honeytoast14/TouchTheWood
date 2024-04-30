using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour, ISavable
{
    public Rigidbody2D rb;
    public Vector2 moveDirection;
    public float moveSpeed = 5f;
    public Animator animator;
    float horizontalInput;
    float verticalInput;
    float movementSpeed;
    public bool canMove = true;
    public bool isMoving;
    public static IsometricPlayerMovementController Instance { get; private set; }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        if (GameController.Instance.state == GameState.FreeRoam)
        {
            Move();
            Animate();
        }

        if (!canMove)
        {
            animator.SetFloat("Speed", 0f);
        }
        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     Debug.Log("call interact");
        //     Interact();
        // }
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
        else
        {
            rb.velocity = Vector2.zero;
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

    public void StopMoving()
    {
        canMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        moveDirection = Vector2.zero;
        // Debug.Log("Stop player from moving");
    }

    public void ResumeMoving()
    {
        canMove = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        // Debug.Log("Player can move now");
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));

        Debug.Log("Facing Direction: " + facingDir);

        var InteractPos = transform.position + facingDir * 2.0f;

        Debug.DrawLine(transform.position, InteractPos, Color.red, 0.3f);

        Debug.Log("Interact method called");
    }

    public object CaptureState()
    {
        float[] position = new float[] { transform.position.x, transform.position.y };
        return position;
    }

    public void RestoreState(object state)
    {
        var position = (float[])state;
        transform.position = new Vector3(position[0], position[1]);
    }
}
