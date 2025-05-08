using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public GameObject StartGame, Options, Exit;
    public Slider volume;
    public TMP_Dropdown resolutionDropdown;
    public Toggle FullscreenToggle;
    public GameObject OptionsScreen;
    public AudioMixer audioMixer;

    private Resolution[] resolutions;

    void Start()
    {
        // Start fullscreen toggle
        FullscreenToggle.isOn = Screen.fullScreen;
        UpdateToggleLabel(FullscreenToggle.isOn);
        FullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        float currentVolume = VolumeManager.Instance.GetVolume();
        volume.value = currentVolume;
        volume.onValueChanged.AddListener(OnVolumeChanged);

        // Populate resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution); //resolution change shit
    }

    void OnVolumeChanged(float volume)
    {
        VolumeManager.Instance.SetVolume(volume); // Called when the slider value changes
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1); // Load the first game scene in index, not 0
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        UpdateToggleLabel(isFullscreen);
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        // Use current fullscreen setting
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    // Update the label that comes with the Toggle
    void UpdateToggleLabel(bool isFullscreen)
    {
        // Access the TextMeshPro label that's attached to the Toggle
        TMP_Text labelText = FullscreenToggle.GetComponentInChildren<TMP_Text>();
        if (labelText != null)
        {
            labelText.text = isFullscreen ? "Fullscreen: ON" : "Fullscreen: OFF";
        }
    }
    public void OptionsButton()
    {
        StartGame.SetActive(false);
        Options.SetActive(false);
        Exit.SetActive(false);
        OptionsScreen.SetActive(true);
    }
    public void BackFromOptions()
    {
        OptionsScreen.SetActive(false);
        StartGame.SetActive(true);
        Options.SetActive(true);
        Exit.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Exited Game");
    }
}
