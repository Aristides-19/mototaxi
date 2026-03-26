using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public class VideoSettingsSc : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private AdvancedDropdown resolutionDropdown;
        [SerializeField] private AdvancedDropdown vsyncDropdown;
        [SerializeField] private AdvancedDropdown fpsDropdown;
        [SerializeField] private AdvancedDropdown fullscreenDropdown;

        private Resolution[] resolutions;

        private void Start()
        {
            // Initialize Resolution Options
            InitializeResolutions();

            // Initialize other UI values based on current settings
            InitializeSettings();

            // Add Listeners
            resolutionDropdown.onChangedValue += SetResolution;
            vsyncDropdown.onChangedValue += SetVSync;
            fpsDropdown.onChangedValue += SetFPS;
            fullscreenDropdown.onChangedValue += SetFullscreen;
        }

        private void OnDestroy()
        {
            // Remove Listeners
            resolutionDropdown.onChangedValue -= SetResolution;
            vsyncDropdown.onChangedValue -= SetVSync;
            fpsDropdown.onChangedValue -= SetFPS;
            fullscreenDropdown.onChangedValue -= SetFullscreen;
        }

        private void InitializeResolutions()
        {
            if (resolutionDropdown == null) return;

            resolutions = Screen.resolutions;

            // Clear existing options
            resolutionDropdown.optionsList.Clear();

            List<string> options = new();
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                // Format: 1920 x 1080 @ 60Hz
                string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + System.Math.Round(resolutions[i].refreshRateRatio.value) + " Hz";
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height &&
                    System.Math.Abs(resolutions[i].refreshRateRatio.value - Screen.currentResolution.refreshRateRatio.value) < 1)
                {
                    currentResolutionIndex = i;
                }
            }

            // Add new options
            resolutionDropdown.AddOptions(options.ToArray());
            resolutionDropdown.SelectOption(currentResolutionIndex);
        }

        private void InitializeSettings()
        {
            // VSync: 0 = Off, 1 = On
            int vsyncVal = QualitySettings.vSyncCount > 0 ? 1 : 0;
            vsyncDropdown.SelectOption(vsyncVal);


            // FPS: 30, 60, 120, 165, Unlimited
            int currentFPS = Application.targetFrameRate;
            int fpsIndex = 4; // Default to Unlimited

            if (currentFPS == 30) fpsIndex = 0;
            else if (currentFPS == 60) fpsIndex = 1;
            else if (currentFPS == 120) fpsIndex = 2;
            else if (currentFPS == 165) fpsIndex = 3;

            fpsDropdown.SelectOption(fpsIndex);


            // Fullscreen: 0 = Fullscreen, 1 = Windowed
            int fsIndex = Screen.fullScreen ? 0 : 1;
            fullscreenDropdown.SelectOption(fsIndex);
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetVSync(int vsyncIndex)
        {
            // vsyncIndex: 0 = Off, 1 = On (Assuming Dropdown order: "Off", "On")
            QualitySettings.vSyncCount = vsyncIndex;
        }

        public void SetFPS(int fpsIndex)
        {
            // Assumed Dropdown Order:
            // 0: 30 FPS
            // 1: 60 FPS
            // 2: 120 FPS
            // 3: 165 FPS
            // 4: Unlimited

            switch (fpsIndex)
            {
                case 0: Application.targetFrameRate = 30; break;
                case 1: Application.targetFrameRate = 60; break;
                case 2: Application.targetFrameRate = 120; break;
                case 3: Application.targetFrameRate = 165; break;
                case 4: Application.targetFrameRate = -1; break; // Unlimited
            }
        }

        public void SetFullscreen(int fullscreenIndex)
        {
            // Assumed Dropdown Order:
            // 0: Fullscreen
            // 1: Windowed

            bool isFullscreen = fullscreenIndex == 0;
            Screen.fullScreen = isFullscreen;
        }
    }
}
