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
    public Toggle vsyncToggle;  // V-Sync Toggle
    public Slider frameRateSlider;  // Frame rate slider
    public GameObject OptionsScreen;
    public AudioMixer audioMixer;

    private Resolution[] resolutions;

    void Start()
    {
        // Start fullscreen toggle
        FullscreenToggle.isOn = Screen.fullScreen;
        FullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        float currentVolume = VolumeManager.Instance.GetVolume();
        volume.value = currentVolume;
        volume.onValueChanged.AddListener(OnVolumeChanged);

        // V-Sync and frame rate settings
        vsyncToggle.isOn = QualitySettings.vSyncCount == 1;
        vsyncToggle.onValueChanged.AddListener(OnVSyncChanged);

        // Set initial frame rate value from PlayerPrefs or default to 60
        frameRateSlider.value = PlayerPrefs.GetFloat("FrameRateCap", 60f);
        frameRateSlider.onValueChanged.AddListener(OnFrameRateChanged);

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
        resolutionDropdown.onValueChanged.AddListener(SetResolution); // Resolution change logic
    }

    void OnVolumeChanged(float volume)
    {
        VolumeManager.Instance.SetVolume(volume); // Called when the slider value changes
    }

    // V-Sync toggle logic
    private void OnVSyncChanged(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSyncEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Frame rate cap slider logic
    private void OnFrameRateChanged(float value)
    {
        Application.targetFrameRate = Mathf.RoundToInt(value);
        PlayerPrefs.SetFloat("FrameRateCap", value);
        PlayerPrefs.Save();
    }


    public void StartButton()
    {
        SceneManager.LoadScene(1); // Load the first game scene in index, not 0
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        // Use current fullscreen setting
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    // Update the label that comes with the Toggle

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

