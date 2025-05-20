using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager _instance;
    public static SceneTransitionManager Instance
    {
        get { return _instance; }
    }

    public string mainSceneName { get; private set; }
    public string miniGameSceneName = "MiniGame";

    private void Awake()
    {
        // Make this a singleton that persists between scenes
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Store the initial scene name
            mainSceneName = SceneManager.GetActiveScene().name;
            Debug.Log($"SceneTransitionManager initialized with main scene: {mainSceneName}");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnterMiniGame()
    {
        // Store the current main scene name
        mainSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Entering mini-game from scene: {mainSceneName}");

        // Load the mini-game scene additively
        SceneManager.LoadSceneAsync(miniGameSceneName, LoadSceneMode.Additive).completed += operation =>
        {
            Scene loadedScene = SceneManager.GetSceneByName(miniGameSceneName);
            if (loadedScene.IsValid())
            {
                SceneManager.SetActiveScene(loadedScene);
                Debug.Log($"Mini-game scene activated. Main scene ({mainSceneName}) is still loaded: {SceneManager.GetSceneByName(mainSceneName).isLoaded}");
            }
        };
    }

    public void ExitMiniGame()
    {
        Debug.Log($"Attempting to exit mini-game. Main scene name: {mainSceneName}");

        // Check if the main scene is loaded
        Scene mainScene = SceneManager.GetSceneByName(mainSceneName);

        if (mainScene.IsValid() && mainScene.isLoaded)
        {
            Debug.Log($"Main scene {mainSceneName} is valid and loaded. Setting as active.");
            SceneManager.SetActiveScene(mainScene);
            SceneManager.UnloadSceneAsync(miniGameSceneName);
        }
        else
        {
            Debug.LogWarning($"Main scene '{mainSceneName}' is not loaded. Loading it first, then unloading mini-game.");
            // Load the main scene and then unload the mini-game
            SceneManager.LoadSceneAsync(mainSceneName, LoadSceneMode.Single);
        }
    }
}