using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject door;
    private Color color;

    private void Start()
    {
        color = GetComponent<SpriteRenderer>().color;
    }

    public void Activate(Color laserColor)
    {
        // Debug.Log("Activating button.");
        if(door != null)
        {
            if(laserColor == color)
            {
                Destroy(door);
            }
            else
            {
                Debug.Log("Color does not match.");
            }
        }
    }
}
