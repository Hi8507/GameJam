using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public int gridWidth = 20;
    public int gridHeight = 20;
    public GameObject snakeSegmentPrefab;
    public GameObject fruitPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public Vector2Int GetRandomGridPosition()
    {
        return new Vector2Int(Random.Range(0, gridWidth), Random.Range(0, gridHeight));
    }
}
