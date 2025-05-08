using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraflip : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the "Player" tag
        if (other.CompareTag("Character"))
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            PlayerMove movement = other.GetComponent<PlayerMove>();
            if (player != null && movement != null)
            {
                movement.SetPaused(true);
                player.SnapCameraHorizontal();
                StartCoroutine(ResumeMovement(movement));
            }
        }

    }

    private IEnumerator ResumeMovement(PlayerMove movement)
    {
        yield return new WaitForSeconds(0.7f); // Wait exactly 2 seconds
        movement.SetPaused(false); // Re-enable movement
        movement.transform.position += new Vector3(5f, 0, 0); // Adjust position to exit trigger
    }


}
