using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    public AudioMixer audioMixer;  // Reference to Audio Mixer
    private const string VolumeParameter = "MasterVolume";  // The exposed parameter in Audio Mixer

    private void Awake()
    {
        // make only one VolumeManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // If another instance exists, destroy this one
        }
    }

    // Set volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(VolumeParameter, Mathf.Log10(volume) * 20);  // Convert volume to dB scale
    }

    // Get the current volume level from the AudioMixer
    public float GetVolume()
    {
        audioMixer.GetFloat(VolumeParameter, out float volume);
        return Mathf.Pow(10, volume / 20);  // Convert dB to linear scale
    }
}
