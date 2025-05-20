using UnityEngine;

public class MiniGameProx : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    private bool playerIsNear = false;

    private void Awake()
    {
        // Ensure we have a SceneTransitionManager
        if (SceneTransitionManager.Instance == null)
        {
            GameObject managerObject = new GameObject("SceneTransitionManager");
            managerObject.AddComponent<SceneTransitionManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(SceneTransitionManager.Instance.playerTag))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(SceneTransitionManager.Instance.playerTag))
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