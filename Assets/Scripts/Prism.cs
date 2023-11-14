using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour
{
    public GameObject laserObject;

    private LineRenderer lineRenderer;
    private Laser laser;
    private Color color;

    private void Start()
    {
        laser = laserObject.GetComponent<Laser>();
        color = GetComponent<SpriteRenderer>().color;
        lineRenderer = laserObject.GetComponent<LineRenderer>();
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    private void Update()
    {
        laserObject.SetActive(false);
    }

    public void Activate()
    {
        laserObject.SetActive(true);
        laser.InitializeLaser();
    }
}
