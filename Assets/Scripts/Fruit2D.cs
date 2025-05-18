using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public static Fruit Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static void SpawnFruit(List<Transform> snakeSegments)
    {
        Vector2Int pos;
        bool valid;

        do
        {
            pos = GameController.Instance.GetRandomGridPosition();
            valid = true;
            foreach (var seg in snakeSegments)
            {
                if ((Vector2)seg.position == new Vector2(pos.x, pos.y))
                {
                    valid = false;
                    break;
                }
            }
        } while (!valid);

        GameObject fruit = Instantiate(GameController.Instance.fruitPrefab);
        fruit.transform.position = new Vector3(pos.x, pos.y, 0);
    }
}
