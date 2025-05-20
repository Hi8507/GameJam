using UnityEngine;

public class MiniGameExit : MonoBehaviour
{
    [SerializeField] private KeyCode exitMiniGameKey = KeyCode.Escape;

    private void Start()
    {
        // Check if SceneTransitionManager exists
        if (SceneTransitionManager.Instance == null)
        {
            Debug.LogError("SceneTransitionManager not found! Mini-game exit may not work properly.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(exitMiniGameKey))
        {
            if (SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.ExitMiniGame();
            }
            else
            {
                Debug.LogError("Cannot exit mini-game: SceneTransitionManager not found!");
            }
        }
    }
}