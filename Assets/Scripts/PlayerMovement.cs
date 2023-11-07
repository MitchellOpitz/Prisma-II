using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveTime = 1f; // Time to move 1 unit.
    private bool isMoving = false;

    private Rigidbody2D rb; // Use Rigidbody2D for 2D movement.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is already moving.
        if (!isMoving)
        {
            // Input from the player.
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate the movement direction.
            Vector2 movement;

            // Check if the horizontal or vertical input is significant (not close to zero).
            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                movement = new Vector2(horizontalInput, 0f).normalized; // Horizontal movement.
            }
            else
            {
                movement = new Vector2(0f, verticalInput).normalized; // Vertical movement.
            }

            if (movement != Vector2.zero)
            {
                // Lock input to prevent further movement during the current move.
                isMoving = true;
                StartCoroutine(MovePlayer(movement));
            }
        }
    }

    IEnumerator MovePlayer(Vector2 direction)
    {
        Vector2 startPos = rb.position;
        Vector2 endPos = startPos + direction;

        float startTime = Time.time;
        float journeyLength = Vector2.Distance(startPos, endPos);
        float speed = journeyLength / moveTime;

        while (Time.time < startTime + moveTime)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / journeyLength;

            rb.position = Vector2.Lerp(startPos, endPos, fractionOfJourney);

            yield return null;
        }

        // Ensure the player ends up at the exact destination.
        rb.position = endPos;

        // Unlock input after completing the move.
        isMoving = false;
    }
}
