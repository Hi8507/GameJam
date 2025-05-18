using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    public int gridWidth = 20;
    public int gridHeight = 20;
    public float padding = 0f; // Optional space around the grid

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        FitGridToCamera();
    }

    void FitGridToCamera()
    {
        float aspectRatio = (float)Screen.width / Screen.height;

        float gridWorldWidth = gridWidth + padding;
        float gridWorldHeight = gridHeight + padding;

        // Calculates the orthographic size needed to fit the grid height
        float orthoSizeHeight = gridWorldHeight / 2f;

        // Calculates orthographic size needed to fit width (adjusted by aspect ratio)
        float orthoSizeWidth = gridWorldWidth / (2f * aspectRatio);

        // Use the larger of the two to ensure full fit
        cam.orthographicSize = Mathf.Max(orthoSizeHeight, orthoSizeWidth);

        cam.transform.position = new Vector3((gridWidth - 1) / 2f, (gridHeight - 1) / 2f, -10f);
    }
}
