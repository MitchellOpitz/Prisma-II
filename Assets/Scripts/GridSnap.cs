using UnityEngine;

public class GridSnap : MonoBehaviour
{
    public float gridSize = 1f; // The size of your grid (1 unit by default).

    void Update()
    {
        // Get the current position of the object.
        Vector3 currentPosition = transform.position;

        // Calculate the new position by snapping to the grid.
        float newX = Mathf.Round(currentPosition.x / gridSize) * gridSize;
        float newY = Mathf.Round(currentPosition.y / gridSize) * gridSize;

        // Update the object's position to the new snapped position.
        transform.position = new Vector3(newX, newY, currentPosition.z);
    }
}
