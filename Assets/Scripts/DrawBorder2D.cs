using UnityEngine;

public class GridBorderDrawer : MonoBehaviour
{
    public GameObject borderTilePrefab;
    public int gridWidth = 20;
    public int gridHeight = 20;

    void Start()
    {
        DrawBorders();
    }

    void DrawBorders()
    {
        for (int x = -1; x <= gridWidth; x++)
        {
            Instantiate(borderTilePrefab, new Vector3(x, -1, 0), Quaternion.identity);
            Instantiate(borderTilePrefab, new Vector3(x, gridHeight, 0), Quaternion.identity);
        }

        for (int y = 0; y < gridHeight; y++)
        {
            Instantiate(borderTilePrefab, new Vector3(-1, y, 0), Quaternion.identity);
            Instantiate(borderTilePrefab, new Vector3(gridWidth, y, 0), Quaternion.identity);
        }
    }
}
