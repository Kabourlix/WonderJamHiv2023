using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Aurore.MainMenu
{
    /// <summary>
    /// Handles the fullscreen option and resolution preferences.
    /// </summary>
    public class GraphicsSettings : MonoBehaviour
    {
        public Toggle FullscreenToggle;
        public TMP_Dropdown ResolutionDropdown;
        public TMP_Dropdown QualityDropdown;
        private bool settingsHaveBeenLoaded = false;

        Resolution[] resolutions;

        void Start()
        {
            ResolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            resolutions = Screen.resolutions;
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " +
                         resolutions[i].height;
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width
                      && resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }

            ResolutionDropdown.AddOptions(options);
            QualityDropdown.ClearOptions();
            QualityDropdown.AddOptions(new List<string>(QualitySettings.names));

            ResolutionDropdown.RefreshShownValue();
            QualityDropdown.RefreshShownValue();

            LoadSettings(currentResolutionIndex);
        }

        public void LoadSettings(int currentResolutionIndex)
        {
            if (PlayerPrefs.HasKey("ResolutionPreference"))
                ResolutionDropdown.value =
                             PlayerPrefs.GetInt("ResolutionPreference");
            else
                ResolutionDropdown.value = currentResolutionIndex;

            if (PlayerPrefs.HasKey("QualityLevel"))
                QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"));

            QualityDropdown.value = QualitySettings.GetQualityLevel();

            FullscreenToggle.isOn = Screen.fullScreen;
            settingsHaveBeenLoaded = true;
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            //Setting resolution when unnecessary will "unmaximize" the window
            if (Screen.currentResolution.width == resolution.width && Screen.currentResolution.height == resolution.height)
            {
                return;
            }
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionPreference", resolutionIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            //We avoid the weird glitch which makes it so this is called on startup and initialized with "false" no matter what happens
            if (!settingsHaveBeenLoaded)
                return;

            if (Screen.fullScreen == isFullscreen)
                return;

            Screen.fullScreen = isFullscreen;
            if (isFullscreen)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }
            PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        }

        public void SetQuality(int qualityIndex)
        {
            if (!settingsHaveBeenLoaded)
                return;

            QualitySettings.SetQualityLevel(qualityIndex);
            PlayerPrefs.SetInt("QualityLevel", qualityIndex);
        }
    }
}