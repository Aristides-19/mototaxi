using Mototaxi.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mototaxi.HUD
{
    public static class ActionsSc
    {
        public static void QuitApplication()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        public static void QuitToMainMenu(ScenesDataSO scenesData)
        {
            SceneManager.LoadScene(scenesData.GetBuildIndex(SceneType.Menu));
            ScoreManagerSc.ResetScore();
        }

        public static void ToggleCursor(bool visible)
        {
            Cursor.visible = visible;
            Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}