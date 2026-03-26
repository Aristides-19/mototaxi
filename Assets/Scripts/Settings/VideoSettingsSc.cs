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

        private const string PREF_RES_WIDTH = "Video_ResWidth";
        private const string PREF_RES_HEIGHT = "Video_ResHeight";
        private const string PREF_RES_REFRESH = "Video_ResRefresh";
        private const string PREF_VSYNC = "Video_VSync";
        private const string PREF_FPS = "Video_FPS";
        private const string PREF_FULLSCREEN = "Video_Fullscreen";

        private void Start()
        {
            // Add Listeners
            resolutionDropdown.onChangedValue += SetResolution;
            vsyncDropdown.onChangedValue += SetVSync;
            fpsDropdown.onChangedValue += SetFPS;
            fullscreenDropdown.onChangedValue += SetFullscreen;

            // Initialize Resolution Options
            InitializeResolutions();

            // Initialize other UI values based on current settings
            InitializeSettings();
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

            bool hasSavedVal = PlayerPrefs.HasKey(PREF_RES_WIDTH) && PlayerPrefs.HasKey(PREF_RES_HEIGHT);
            int savedWidth = PlayerPrefs.GetInt(PREF_RES_WIDTH);
            int savedHeight = PlayerPrefs.GetInt(PREF_RES_HEIGHT);
            int savedRefresh = PlayerPrefs.GetInt(PREF_RES_REFRESH, 0);

            for (int i = 0; i < resolutions.Length; i++)
            {
                // Format: 1920 x 1080 @ 60Hz
                double refreshRate = resolutions[i].refreshRateRatio.value;
                string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + System.Math.Round(refreshRate) + " Hz";
                options.Add(option);

                if (hasSavedVal)
                {
                    if (resolutions[i].width == savedWidth &&
                        resolutions[i].height == savedHeight &&
                        (savedRefresh == 0 || System.Math.Abs(System.Math.Round(refreshRate) - savedRefresh) < 1))
                    {
                        currentResolutionIndex = i;
                    }
                }
                else
                {
                    if (resolutions[i].width == Screen.currentResolution.width &&
                        resolutions[i].height == Screen.currentResolution.height &&
                        System.Math.Abs(resolutions[i].refreshRateRatio.value - Screen.currentResolution.refreshRateRatio.value) < 1)
                    {
                        currentResolutionIndex = i;
                    }
                }
            }

            // Add new options
            resolutionDropdown.AddOptions(options.ToArray());
            resolutionDropdown.SelectOption(currentResolutionIndex);
        }

        private void InitializeSettings()
        {
            // VSync: 0 = Off, 1 = On
            int vsyncVal;
            if (PlayerPrefs.HasKey(PREF_VSYNC))
            {
                vsyncVal = PlayerPrefs.GetInt(PREF_VSYNC);
            }
            else
            {
                vsyncVal = QualitySettings.vSyncCount > 0 ? 1 : 0;
            }
            vsyncDropdown.SelectOption(vsyncVal);


            // FPS: 30, 60, 120, 165, Unlimited
            int fpsIndex = 4; // Default to Unlimited

            if (PlayerPrefs.HasKey(PREF_FPS))
            {
                fpsIndex = PlayerPrefs.GetInt(PREF_FPS);
            }
            else
            {
                int currentFPS = Application.targetFrameRate;
                if (currentFPS == 30) fpsIndex = 0;
                else if (currentFPS == 60) fpsIndex = 1;
                else if (currentFPS == 120) fpsIndex = 2;
                else if (currentFPS == 165) fpsIndex = 3;
            }

            fpsDropdown.SelectOption(fpsIndex);


            // Fullscreen: 0 = Fullscreen, 1 = Windowed
            int fsIndex;
            if (PlayerPrefs.HasKey(PREF_FULLSCREEN))
            {
                fsIndex = PlayerPrefs.GetInt(PREF_FULLSCREEN) == 1 ? 0 : 1;
            }
            else
            {
                fsIndex = Screen.fullScreen ? 0 : 1;
            }
            fullscreenDropdown.SelectOption(fsIndex);
        }

        public void SetResolution(int resolutionIndex)
        {
            if (resolutions == null || resolutions.Length == 0) return;
            if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length) return;

            Resolution resolution = resolutions[resolutionIndex];
            int refreshHz = Mathf.RoundToInt((float)resolution.refreshRateRatio.value);

            // FullScreenWindow avoids monitor mode switches that can alter brightness/gamma on some laptops.
            FullScreenMode mode = Screen.fullScreenMode == FullScreenMode.Windowed
                ? FullScreenMode.Windowed
                : FullScreenMode.FullScreenWindow;

            if (mode == FullScreenMode.Windowed)
            {
                // In windowed mode, apply the requested client size directly.
                Screen.SetResolution(resolution.width, resolution.height, mode, resolution.refreshRateRatio);
                ScalableBufferManager.ResizeBuffers(1f, 1f);
            }
            else
            {
                // In fullscreen, keep native desktop output and only scale internal rendering.
                int nativeWidth = Display.main != null ? Display.main.systemWidth : Screen.currentResolution.width;
                int nativeHeight = Display.main != null ? Display.main.systemHeight : Screen.currentResolution.height;

                Screen.SetResolution(nativeWidth, nativeHeight, FullScreenMode.FullScreenWindow, Screen.currentResolution.refreshRateRatio);

                float widthScale = (float)resolution.width / nativeWidth;
                float heightScale = (float)resolution.height / nativeHeight;
                float renderScale = Mathf.Clamp(Mathf.Min(widthScale, heightScale), 0.5f, 1f);
                ScalableBufferManager.ResizeBuffers(renderScale, renderScale);
            }

            PlayerPrefs.SetInt(PREF_RES_WIDTH, resolution.width);
            PlayerPrefs.SetInt(PREF_RES_HEIGHT, resolution.height);
            PlayerPrefs.SetInt(PREF_RES_REFRESH, refreshHz);
            PlayerPrefs.Save();
        }

        public void SetVSync(int vsyncIndex)
        {
            // vsyncIndex: 0 = Off, 1 = On (Assuming Dropdown order: "Off", "On")
            QualitySettings.vSyncCount = vsyncIndex;

            PlayerPrefs.SetInt(PREF_VSYNC, vsyncIndex);
            PlayerPrefs.Save();
        }

        public void SetFPS(int fpsIndex)
        {
            SetFPSInternal(fpsIndex);

            PlayerPrefs.SetInt(PREF_FPS, fpsIndex);
            PlayerPrefs.Save();
        }

        private void SetFPSInternal(int fpsIndex)
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
            Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

            PlayerPrefs.SetInt(PREF_FULLSCREEN, isFullscreen ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
