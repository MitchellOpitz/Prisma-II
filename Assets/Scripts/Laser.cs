using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform emitter;

    private LineRenderer lineRenderer;
    private Vector3 laserDirection;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.SetPosition(0, emitter.position);
        laserDirection = transform.up;
        RaycastHit2D hit;

        // Perform a 2D raycast
        hit = Physics2D.Raycast(transform.position, laserDirection);

        if (hit.collider != null)
        {
            Vector3 hitPosition = new Vector3(hit.point.x, hit.point.y, 0f);
            lineRenderer.SetPosition(1, hitPosition);
        }
    }
}
