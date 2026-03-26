using Mototaxi.Core;
using Mototaxi.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mototaxi.HUD
{
    public class PauseMenuSc : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Reference to the Pause Menu Canvas Group")]
        [SerializeField] Canvas pauseMenuCanvas;

        [Header("Input Settings")]
        [Tooltip("Reference to the Input Action Asset (Player/Pause)")]
        [SerializeField] InputActionsSO inputActions;

        [Header("Scene Management")]
        [SerializeField] ScenesDataSO scenesData;

        public static bool IsPaused { get; private set; } = false;

        private void Awake()
        {
            ResetPauseState();
            inputActions.PauseAction.action.performed += OnPausePerformed;
        }

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            TogglePause();
        }

        private void TogglePause()
        {
            if (IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        private void PauseGame()
        {
            IsPaused = true;
            pauseMenuCanvas.transform.gameObject.SetActive(true);

            Time.timeScale = 0f;
            AudioListener.pause = true;

            ActionsSc.ToggleCursor(true);
        }

        public void ResumeGame()
        {
            pauseMenuCanvas.transform.gameObject.SetActive(false);

            ResetPauseState();

            ActionsSc.ToggleCursor(false);
        }

        public void QuitToMainMenu()
        {
            ResetPauseState();
            ActionsSc.QuitToMainMenu(scenesData);
        }

        public void QuitApplication()
        {
            ResetPauseState();
            ActionsSc.QuitApplication();
        }

        public void RestrictPause()
        {
            inputActions.PauseAction.action.performed -= OnPausePerformed;
        }

        private void OnDestroy()
        {
            ResetPauseState();
            inputActions.PauseAction.action.performed -= OnPausePerformed;
        }

        private void ResetPauseState()
        {
            IsPaused = false;
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }
}