using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveTime = 1f; // Time to move 1 unit.
    private bool isMoving = false;

    private Rigidbody2D rb; // Use Rigidbody2D for 2D movement.
    public float movementDistance = 1f; // Distance to check for obstacles.

    // Tags to exclude from restricting player movement.
    public string[] tagsToExclude;

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
            float button1Input = Input.GetAxis("Fire1");
            float button2Input = Input.GetAxis("Fire2");

            if (button1Input > 0 || button2Input > 0)
            {
                RotateMirror(button1Input > 0 ? 1 : -1);
                return;
            }

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
                // Rotate the player to face the movement direction.
                float targetRotation = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, targetRotation);

                // Calculate the target position.
                Vector2 targetPosition = rb.position + movement * movementDistance;

                // Check for colliders in the movement direction.
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(targetPosition, 0.2f); // Adjust the radius as needed.

                bool canMove = true;

                // Check if any of the colliders are not the player's collider and not in the excluded tags.
                foreach (var collider in hitColliders)
                {
                    if (collider.gameObject != gameObject && !IsTagExcluded(collider.gameObject.tag))
                    {
                        // If the collider has the "Mirror" tag, move the mirror.
                        if (collider.gameObject.CompareTag("Mirror"))
                        {
                            Mirror mirror = collider.GetComponent<Mirror>();
                            if (mirror.CheckDirection(movement))
                            {
                                mirror.MoveMirror(movement);
                                isMoving = true;
                                StartCoroutine(MovePlayer(movement));
                            }
                        }

                        canMove = false;
                        break;
                    }
                }

                if (canMove)
                {
                    // Lock input to prevent further movement during the current move.
                    isMoving = true;
                    StartCoroutine(MovePlayer(movement));
                }
            }
        }
    }

    void RotateMirror(int direction)
    {
        // Cast a ray in the forward direction of the player.
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, 1f);

        // Debug line to visualize the raycast.
        Debug.DrawRay(transform.position, transform.right * 1f, Color.green, 1f);

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Mirror") && !hit.collider.CompareTag("Player"))
            {
                Debug.Log("Mirror found!");
                Mirror mirror = hit.collider.GetComponent<Mirror>();
                mirror.RotateMirror(direction);
            }
        }
    }

    IEnumerator MovePlayer(Vector2 direction)
    {
        // Move the player.
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

    bool IsTagExcluded(string tag)
    {
        foreach (var excludedTag in tagsToExclude)
        {
            if (tag == excludedTag)
            {
                return true;
            }
        }
        return false;
    }
}
