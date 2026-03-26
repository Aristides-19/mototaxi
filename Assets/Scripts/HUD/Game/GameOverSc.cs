using System;
using ArcadeBP_Pro;
using Mototaxi.Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mototaxi.HUD
{
    public class GameOverSc : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Reference to the Game Over Canvas")]
        [SerializeField] Canvas gameOverCanvas;
        [SerializeField] TextMeshProUGUI statsText;

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

            if (ragdollActivator != null)
            {
                ragdollActivator.OnRagdollActivated += HandleCrash;
            }
            else
            {
                Debug.LogWarning("GameEndSc: RagdollActivator reference is missing.");
            }

            TimeManagerSc.OnTimeUp += HandleCrash;
        }

        private void HandleCrash()
        {
            Invoke(nameof(ShowGameOver), gameOverDelay);
        }

        private void ShowGameOver()
        {
            TimeSpan t = TimeSpan.FromSeconds(TimeManagerSc.ElapsedTime);
            statsText.text = $"Bs. {ScoreManagerSc.CurrentScore} en {t.Minutes:D2}:{t.Seconds:D2}";
            gameOverCanvas.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }

        public void QuitToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(scenesData.GetBuildIndex(SceneType.Menu));

        }

        public void QuitApplication()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}