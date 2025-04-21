using UnityEngine;

public class CameraRoomTransition : MonoBehaviour
{
    public Camera mainCamera;
    public string roomLayerName = "room";  // Layer name for the room
    public WallFade[] wallFades;  // Reference to the WallFade script (array for multiple walls)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))  // Check if the player enters
        {
            // Fade out the walls when the player enters
            foreach (var wall in wallFades)
            {
                wall.FadeOut();
            }

            // Hide walls from camera (optional, if you want to stop rendering them)
            mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("wall"));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))  // Check if the player exits
        {
            // Fade in the walls when the player exits
            foreach (var wall in wallFades)
            {
                wall.FadeIn();
            }

            // Show walls again from the camera
            mainCamera.cullingMask |= (1 << LayerMask.NameToLayer("wall"));
        }
    }
}
