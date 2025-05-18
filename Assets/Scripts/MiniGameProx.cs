using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameProx : MonoBehaviour
{
    [SerializeField] private int miniGameSceneBuildIndex = 6;  // build index of the mini-game scene
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private KeyCode exitMiniGameKey = KeyCode.Escape;  // key to exit the mini-game

    private bool playerIsNear = false;
    private bool inMiniGame = false;
    private Scene mainScene;

    private void Start()
    {
        // Store the current scene
        mainScene = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            playerIsNear = false;
        }
    }

    private void Update()
    {
        // Only process main scene input when the main scene is active
        if (!inMiniGame)
        {
            if (playerIsNear && Input.GetKeyDown(interactionKey))
            {
                EnterMiniGame();
            }
        }
        // Only process mini-game scene input when in the mini-game
        else if (inMiniGame && Input.GetKeyDown(exitMiniGameKey))
        {
            ExitMiniGame();
        }
    }

    private void EnterMiniGame()
    {
        // Load the mini-game scene additively
        SceneManager.LoadSceneAsync(miniGameSceneBuildIndex, LoadSceneMode.Additive).completed += operation => {
            // Set the mini-game scene as the active scene after it's loaded
            Scene miniGameScene = SceneManager.GetSceneByBuildIndex(miniGameSceneBuildIndex);
            SceneManager.SetActiveScene(miniGameScene);

            // script will continue running, but the main scene is no longer active
            inMiniGame = true;
        };
    }

    private void ExitMiniGame()
    {
        // Set main scene as active before unloading the mini-game
        SceneManager.SetActiveScene(mainScene);

        // Unload the mini-game scene
        SceneManager.UnloadSceneAsync(miniGameSceneBuildIndex);
        inMiniGame = false;
    }
}