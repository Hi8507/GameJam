using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character")) // Only trigger for player
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            SceneManager.LoadScene(nextSceneIndex);
            
        }
    }
}