using UnityEngine;

public class MiniGameExit : MonoBehaviour
{
    [SerializeField] private KeyCode exitMiniGameKey = KeyCode.Escape;

    private void Start()
    {
        // Verify SceneTransitionManager exists
        if (SceneTransitionManager.Instance == null)
        {
            Debug.LogError("SceneTransitionManager not found in mini-game scene! Creating one...");
            GameObject managerObject = new GameObject("SceneTransitionManager");
            managerObject.AddComponent<SceneTransitionManager>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(exitMiniGameKey))
        {
            SceneTransitionManager.Instance.ExitMiniGame();
        }
    }
}