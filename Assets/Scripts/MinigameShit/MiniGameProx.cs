using UnityEngine;

public class MiniGameProx : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    private bool playerIsNear = false;

    private void Start()
    {
        // Ensure there's a SceneTransitionManager in the scene
        if (SceneTransitionManager.Instance == null)
        {
            GameObject manager = new GameObject("SceneTransitionManager");
            manager.AddComponent<SceneTransitionManager>();
        }
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
        if (playerIsNear && Input.GetKeyDown(interactionKey))
        {
            SceneTransitionManager.Instance.EnterMiniGame();
        }
    }
}