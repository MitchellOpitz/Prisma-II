using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class Mirror : MonoBehaviour
{
    public float moveTime = 1f; // Time to move 1 unit.
    public float rotationTime = 1f;

    private GridSnap gridSnap; // Assuming GridSnap is another script attached to the same GameObject

    private bool isMoving = false;

    // Reference to the Tilemap containing predefined paths.
    public Tilemap pathTilemap;
    public Tilemap rotationTilemap;

    private void GetGridSnap()
    {
        gridSnap = GetComponent<GridSnap>();
    }

    public bool CheckDirection(Vector2 direction)
    {
        GetGridSnap();

        // Calculate the target position.
        Vector2 targetPosition = (Vector2)transform.position + direction;

        // Convert the target position to a cell position on the Tilemap.
        Vector3Int cellPosition = pathTilemap.WorldToCell(targetPosition);

        // Check if the cell contains a specific tile that allows movement.
        TileBase pathTile = pathTilemap.GetTile(cellPosition);
        TileBase rotationTile = rotationTilemap.GetTile(cellPosition);

        // Customize this condition based on your tile setup.
        // For example, you might have specific tiles that allow movement.
        return pathTile != null || rotationTile != null;
    }

    public void MoveMirror(Vector2 direction)
    {
        if (!isMoving && CheckDirection(direction))
        {
            // Remove the GridSnap component
            Destroy(gridSnap);

            // Start moving the mirror over a set amount of time
            StartCoroutine(MoveOverTime(direction));
        }
    }

    public void RotateMirror(int direction)
    {
        if (!isMoving && CheckRotationTile())
        {
            StartCoroutine(RotateOverTime(direction));
        }
    }

    private bool CheckRotationTile()
    {
        GetGridSnap();

        // Convert the target position to a cell position on the Tilemap.
        Vector3Int cellPosition = rotationTilemap.WorldToCell(transform.position);

        // Check if the cell contains a specific tile that allows rotation.
        TileBase tile = rotationTilemap.GetTile(cellPosition);

        // Customize this condition based on your tile setup.
        // For example, you might have a specific tile that allows rotation.
        return tile != null;
    }

    private IEnumerator RotateOverTime(int direction)
    {
        isMoving = true;

        float rotationAmount = 15f * direction;
        Quaternion initialRotation = transform.rotation;
        float targetRotationZ = Mathf.Round(initialRotation.eulerAngles.z / 15f) * 15f + rotationAmount;

        float elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            float currentRotationZ = Mathf.Lerp(initialRotation.eulerAngles.z, targetRotationZ, elapsedTime / rotationTime);
            transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly on intervals of 15 degrees.
        transform.rotation = Quaternion.Euler(0, 0, targetRotationZ);

        isMoving = false;
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
