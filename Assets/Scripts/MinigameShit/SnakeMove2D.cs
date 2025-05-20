using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;

public class SnakeMove2D : MonoBehaviour
{
    public Vector2Int direction = Vector2Int.right;
    public float moveInterval = 0.2f;
    private float moveTimer;

    private List<Transform> segments = new List<Transform>();
    private Vector2Int gridPosition;

    void Start()
    {
        gridPosition = new Vector2Int(10, 10);
        transform.position = GridToWorld(gridPosition);
        segments.Add(this.transform);
        Fruit.SpawnFruit(new List<Transform> { this.transform });

    }

    void Update()
    {
        HandleInput();

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval)
        {
            moveTimer = 0f;
            Move();
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2Int.down) direction = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S) && direction != Vector2Int.up) direction = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A) && direction != Vector2Int.right) direction = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) && direction != Vector2Int.left) direction = Vector2Int.right;
    }

    Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
    }
    void Move()
    {
        Vector2Int newHeadPos = gridPosition + direction;

        // Check for collision with walls
        if (newHeadPos.x < 0 || newHeadPos.x >= GameController.Instance.gridWidth ||
            newHeadPos.y < 0 || newHeadPos.y >= GameController.Instance.gridHeight)
        {
            Die();
            return;
        }

        // Check for self-collision
        for (int i = 0; i < segments.Count; i++)
        {
            // Skip the head (i = 0)
            if (i == 0) continue;

            if (WorldToGrid(segments[i].position) == newHeadPos)
            {
                Die();
                return;
            }
        }

        // Move body
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        // Move head
        gridPosition = newHeadPos;
        transform.position = GridToWorld(gridPosition);

        // Check fruit collision
        if (Fruit.Instance != null && (Vector2)transform.position == (Vector2)Fruit.Instance.transform.position)
        {
            Grow();
            Destroy(Fruit.Instance.gameObject);
            Fruit.SpawnFruit(segments);
        }
    }

    void Grow()
    {
        GameObject segment = Instantiate(GameController.Instance.snakeSegmentPrefab);
        segment.transform.position = segments[segments.Count - 1].position;
        segments.Add(segment.transform);
    }

    void Die()
    {
        Debug.Log("Snake Died!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(6); // reload scene
    }

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y, 0);
    }
}
