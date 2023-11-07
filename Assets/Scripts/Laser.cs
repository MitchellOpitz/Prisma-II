using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform emitter;
    public int maxBounces = 10; // Set a maximum number of bounces

    private LineRenderer lineRenderer;
    private List<Vector3> laserPositions = new List<Vector3>();
    private Vector3 laserDirection;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        InitializeLaser();
    }

    private void Update()
    {
        laserPositions.Clear(); // Clear the list before updating
        laserPositions.Add(emitter.position);
        ShootLaser(emitter.position, laserDirection, 0);
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

        if (hit.collider != null)
        {
            Vector3 hitPosition = new Vector3(hit.point.x, hit.point.y, 0f);
            laserPositions.Add(hitPosition);

            if (hit.collider.tag == "Mirror")
            {
                Vector3 mirrorNormal = hit.transform.up; // Adjust this according to your mirror orientation
                Vector3 newDirection = Vector3.Reflect(direction, mirrorNormal);
                ShootLaser(hitPosition, newDirection, bounces + 1);
            }
        }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = laserPositions.Count;
        lineRenderer.SetPositions(laserPositions.ToArray());
    }
}
