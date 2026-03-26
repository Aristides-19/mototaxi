using System;
using ArcadeBP_Pro;
using Mototaxi.Core;
using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    public class GameOverSc : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Reference to the Game Over Canvas")]
        [SerializeField] Canvas gameOverCanvas;
        [SerializeField] TextMeshProUGUI statsText;
        [SerializeField] PauseMenuSc pauseMenu;

        [Header("Settings")]
        [Tooltip("Time delay in seconds before showing the Game Over screen after a crash")]
        [SerializeField] float gameOverDelay = 2.0f;

        [Header("Scene Management")]
        [SerializeField] ScenesDataSO scenesData;

        [Header("Dependencies")]
        [Tooltip("Reference to the RagdollActivator component on the bike")]
        [SerializeField] RagdollActivator ragdollActivator;

        private void Awake()
        {
            gameOverCanvas.gameObject.SetActive(false);

            if (ragdollActivator == null) Debug.LogWarning("GameOverSc: RagdollActivator reference is missing.");
        }

        private void OnEnable()
        {
            TimeManagerSc.OnTimeUp += HandleCrash;
            ragdollActivator.OnRagdollActivated += HandleCrash;
        }

        private void OnDisable()
        {
            TimeManagerSc.OnTimeUp -= HandleCrash;
            ragdollActivator.OnRagdollActivated -= HandleCrash;
        }

        private void HandleCrash()
        {
            Invoke(nameof(ShowGameOver), gameOverDelay);
        }

        private void ShowGameOver()
        {
            pauseMenu.RestrictPause();
            pauseMenu.ResumeGame();

            TimeSpan t = TimeSpan.FromSeconds(TimeManagerSc.ElapsedTime);
            statsText.text = $"Bs. {ScoreManagerSc.CurrentScore} en {t.Minutes:D2}:{t.Seconds:D2}";
            gameOverCanvas.gameObject.SetActive(true);

            ActionsSc.ToggleCursor(true);

        }

        public void QuitToMainMenu()
        {
            ActionsSc.QuitToMainMenu(scenesData);
        }

        public void QuitApplication()
        {
            ActionsSc.QuitApplication();
        }
    }
}