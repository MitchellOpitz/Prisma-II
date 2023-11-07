using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to set the movement speed.

    private Rigidbody2D rb; // Use Rigidbody2D for 2D movement.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input from the player.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction.
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

        // Move the player using Rigidbody2D.
        rb.velocity = movement * moveSpeed;
    }
}
