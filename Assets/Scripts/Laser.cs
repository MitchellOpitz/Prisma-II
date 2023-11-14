using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform emitter;
    public float rendererOffset;
    public int maxBounces = 10; // Set a maximum number of bounces
    public List<string> tagsToIgnore = new List<string>(); // Specify the tags to ignore

    private LineRenderer lineRenderer;
    private List<Vector3> laserPositions = new List<Vector3>();
    private Vector3 laserDirection;
    private HashSet<Transform> processedMirrors = new HashSet<Transform>();

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        InitializeLaser();
    }

    private void Update()
    {
        ResetProcessedMirrors(); // Reset the processed mirrors at the beginning of each frame
        laserPositions.Clear(); // Clear the list before updating
        Vector3 startPosition = emitter.position + new Vector3(0, rendererOffset, 0);
        laserPositions.Add(startPosition);
        ShootLaser(startPosition, laserDirection, 0);
        UpdateLineRenderer();
    }

    private void InitializeLaser()
    {
        laserPositions.Add(emitter.position);
        laserPositions.Add(emitter.position);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, emitter.position);
        lineRenderer.SetPosition(1, emitter.position);
        laserDirection = transform.up;
    }

    private void ShootLaser(Vector3 startingPosition, Vector3 direction, int bounces)
    {
        if (bounces >= maxBounces)
        {
            return; // Exit if we've reached the maximum number of bounces
        }

        RaycastHit2D hit = Physics2D.Raycast(startingPosition, direction);

        if (hit.collider != null && tagsToIgnore.Contains(hit.collider.tag))
        {
            // Continue with the raycast as if the object with the ignored tag doesn't exist
        }

        if (hit.collider != null)
        {
            Vector3 hitPosition = new Vector3(hit.point.x, hit.point.y, 0f);

            // Adjust the hit position slightly in the opposite direction
            hitPosition -= direction.normalized * 0.00001f;

            laserPositions.Add(hitPosition);

            if (hit.collider.tag == "Mirror" && !processedMirrors.Contains(hit.transform))
            {
                processedMirrors.Add(hit.transform); // Mark the mirror as processed

                Vector3 mirrorNormal = hit.transform.up;
                Vector3 newDirection = Vector3.Reflect(direction, mirrorNormal);
                ShootLaser(hitPosition, newDirection, bounces + 1);
            }

            if(hit.collider.tag == "Button")
            {
                Button button = hit.collider.GetComponent<Button>();
                button.Activate();
            }
        }
    }



    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = laserPositions.Count;
        lineRenderer.SetPositions(laserPositions.ToArray());
    }

    // Call this function at the beginning of Update to reset processedMirrors
    private void ResetProcessedMirrors()
    {
        processedMirrors.Clear();
    }
}
