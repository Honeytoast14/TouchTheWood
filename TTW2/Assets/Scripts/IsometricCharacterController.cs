using System.Collections.Generic;
using UnityEngine;

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
    public LayerMask pushableLayer;
    [SerializeField] List<AudioClip> walk;
    AudioSource walkAudioSource;
    float walkSoundTimer = 0f;
    float walkSoundInterval = 0.67f;

    public static IsometricPlayerMovementController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        walkAudioSource = GetComponent<AudioSource>();
        walkAudioSource.loop = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (GameController.Instance.state == GameState.FreeRoam)
        {
            Move();
            Animate();
        }

        if (!canMove)
        {
            animator.SetFloat("Speed", 0f);
            if (walkAudioSource.isPlaying)
            {
                walkAudioSource.Stop();
            }
        }

        if (walkAudioSource.isPlaying)
        {
            walkSoundTimer += Time.deltaTime;
        }
    }

    void Move()
    {
        if (canMove)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            rb.velocity = new Vector2(horizontalInput, verticalInput) * moveSpeed;

            moveDirection = new Vector2(horizontalInput, verticalInput);
            movementSpeed = Mathf.Clamp(moveDirection.magnitude, 0.0f, 1.0f);

            if (moveDirection != Vector2.zero)
            {
                if (!walkAudioSource.isPlaying || walkSoundTimer >= walkSoundInterval)
                {
                    PlayRandomWalkClip();
                    walkAudioSource.Play();
                    walkSoundTimer = 0f;
                }
            }
            else
            {
                if (walkAudioSource.isPlaying)
                {
                    walkAudioSource.Stop();
                }
            }
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

    void PlayRandomWalkClip()
    {
        if (walk.Count > 0)
        {
            walkAudioSource.clip = walk[Random.Range(0, walk.Count)];
        }
    }

    public void StopMoving()
    {
        canMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        moveDirection = Vector2.zero;

        if (walkAudioSource.isPlaying)
        {
            walkAudioSource.Stop();
        }
    }

    public void ResumeMoving()
    {
        canMove = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
