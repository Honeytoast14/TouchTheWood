using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour, ISavable
{
    public LayerMask SolidObjectLayer;
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

    public void Update()
    {
        if (IsWalkable(rb.position))
        {
            Move();
            Animate();
        }
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

    private bool IsWalkable(Vector2 moveDirection)
    {
        if (Physics2D.OverlapCircle(moveDirection, 0f, SolidObjectLayer) != null)
        {
            return false;
        }
        return true;
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