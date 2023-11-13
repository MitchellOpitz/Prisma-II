using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour
{
    public float moveTime = 1f; // Time to move 1 unit.

    private GridSnap gridSnap; // Assuming GridSnap is another script attached to the same GameObject

    private bool isMoving = false;

    private void GetGridSnap()
    {
        gridSnap = GetComponent<GridSnap>();
    }

    public void MoveMirror(Vector2 direction)
    {
        GetGridSnap();
        if (!isMoving)
        {
            // Visualize the raycast
            Debug.DrawRay(transform.position, direction, Color.red, 1.0f);

            // Use Physics2D.RaycastNonAlloc to get all hits
            RaycastHit2D[] hits = new RaycastHit2D[1];
            int hitCount = Physics2D.RaycastNonAlloc(transform.position, direction, hits, 1f);

            // Check each hit
            for (int i = 0; i < hitCount; i++)
            {
                // Ignore hits from the same GameObject
                if (hits[i].collider.gameObject == gameObject)
                {
                    continue;
                }

                // If there is a collider from another object, do nothing
                Debug.Log(hits[i].collider.gameObject);
                return;
            }

            // If there are no hits or hits from other objects, continue with the movement
            // Remove the GridSnap component
            Destroy(gridSnap);

            // Start moving the mirror over a set amount of time
            StartCoroutine(MoveOverTime(direction));
        }
    }

    private IEnumerator MoveOverTime(Vector2 direction)
    {
        isMoving = true;

        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(direction.x, direction.y, 0);

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Add back the GridSnap component
        gridSnap = gameObject.AddComponent<GridSnap>();

        isMoving = false;
    }
}
