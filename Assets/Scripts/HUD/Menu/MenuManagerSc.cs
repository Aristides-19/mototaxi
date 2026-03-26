using System.Collections;
using System.Collections.Generic;
using Mototaxi.Core;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Mototaxi.HUD
{
    public class MenuManagerSc : MonoBehaviour
    {
        [Header("Navigation Settings")]
        [SerializeField] AnimatedMenuSc mainMenu;
        [SerializeField] ScenesDataSO scenesData;
        [SerializeField] CinemachineCamera settingsCam;
        private readonly Stack<AnimatedMenuSc> stack = new();

        [Header("Loading Settings")]
        [SerializeField] GameObject loadingPanel;
        [SerializeField] Slider progressBar;

        void Start()
        {
            if (mainMenu != null) Go(mainMenu);
            else Debug.LogError("Main Menu is not assigned in MenuManager.");
        }

        public void Go(AnimatedMenuSc target)
        {
            if (stack.Count > 0) stack.Peek().SetState(false);

            target.SetState(true);
            stack.Push(target);
        }

        public void Back()
        {
            if (stack.Count <= 1) return;

            stack.Pop().SetState(false);
            stack.Peek().SetState(true);
        }

        public void Quit()
        {
            ActionsSc.QuitApplication();
        }

        public void ChangeToSettingsCamera()
        {
            settingsCam.Priority = 20;
        }

        public void ChangeToMainCamera()
        {
            settingsCam.Priority = 0;
        }

        public void LoadGameScene()
        {
            StartCoroutine(LoadAsync(SceneType.Road));
        }

        private readonly WaitForSeconds wait = new(1f);
        private IEnumerator LoadAsync(SceneType scene)
        {
            mainMenu.SetState(false, immediate: true);
            loadingPanel.SetActive(true);
            yield return null;

            AsyncOperation op = SceneManager.LoadSceneAsync(scenesData.GetBuildIndex(scene));

            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                progressBar.value = Mathf.Clamp01(op.progress / 0.9f);
                yield return null;
            }

            progressBar.value = 1f;
            yield return wait;

            op.allowSceneActivation = true;
        }
    }

}