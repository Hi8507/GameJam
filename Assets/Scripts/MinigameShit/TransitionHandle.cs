using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager _instance;
    public static SceneTransitionManager Instance
    {
        get { return _instance; }
    }

    [HideInInspector] public string mainSceneName;
    public string miniGameSceneName = "MiniGame";

    // Player state data
    private Vector3 playerPosition;
    private Quaternion playerRotation;
    private string playerSceneName;

    // Reference to the player GameObject tag
    public string playerTag = "Character";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we're returning to the main scene, restore player position
        if (scene.name == playerSceneName)
        {
            StartCoroutine(RestorePlayerPosition());
        }
    }

    public void EnterMiniGame()
    {
        // Find and store player position
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            // Store player data
            playerPosition = player.transform.position;
            playerRotation = player.transform.rotation;
            playerSceneName = SceneManager.GetActiveScene().name;

            Debug.Log($"Saved player position: {playerPosition} in scene: {playerSceneName}");
        }
        else
        {
            Debug.LogWarning($"Player with tag '{playerTag}' not found!");
        }

        // Load mini-game scene
        SceneManager.LoadSceneAsync(miniGameSceneName);
    }

    public void ExitMiniGame()
    {
        if (!string.IsNullOrEmpty(playerSceneName))
        {
            // Return to the scene the player was in
            SceneManager.LoadSceneAsync(playerSceneName);
        }
        else
        {
            Debug.LogWarning("No previous scene recorded. Cannot return player.");
        }
    }

    private IEnumerator RestorePlayerPosition()
    {
        // Wait for scene to fully load
        yield return new WaitForEndOfFrame();

        // Find player in new scene
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            // Restore position and rotation
            player.transform.position = playerPosition;
            player.transform.rotation = playerRotation;

            Debug.Log($"Restored player position to: {playerPosition}");

            // If your player controller gets disabled during teleport, re-enable it here
            // Example:
            var playerController = player.GetComponent<CharacterController>();
            if (playerController != null)
                playerController.enabled = true;
        }
        else
        {
            Debug.LogWarning($"Player with tag '{playerTag}' not found in restored scene!");
        }
    }
}