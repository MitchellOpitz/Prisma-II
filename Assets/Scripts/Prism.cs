using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour
{
    public GameObject laserObject;

    private LineRenderer lineRenderer;
    private Laser laser;
    private Color color;

    private void Update()
    {
        laserObject.SetActive(false);
        laser = laserObject.GetComponent<Laser>();
        color = GetComponent<SpriteRenderer>().color;
        lineRenderer = laserObject.GetComponent<LineRenderer>();
    }

    public void Activate()
    {
        laserObject.SetActive(true);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        laser.InitializeLaser();
    }
}
