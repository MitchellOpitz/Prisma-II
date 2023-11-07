using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridSnap))]
public class GridSnapEditor : Editor
{
    private void OnSceneGUI()
    {
        GridSnap gridSnap = (GridSnap)target;
        SnapToGrid(gridSnap);
    }

    private void SnapToGrid(GridSnap gridSnap)
    {
        float gridSize = gridSnap.gridSize;
        Vector3 currentPosition = gridSnap.transform.position;

        float newX = Mathf.Round(currentPosition.x / gridSize) * gridSize;
        float newY = Mathf.Round(currentPosition.y / gridSize) * gridSize;

        gridSnap.transform.position = new Vector3(newX, newY, currentPosition.z);
    }
}
