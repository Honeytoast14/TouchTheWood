using UnityEngine;
using UnityEngine.Tilemaps;

public class PushableBox : MonoBehaviour
{
    public float pushForce = 2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Push(Vector2 pushDirection)
    {
        rb.velocity = pushDirection * pushForce;
    }
}
