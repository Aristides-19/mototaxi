using UnityEngine;
using Mototaxi.Core;

namespace Mototaxi.HUD
{
    public class SceneLoadTriggerSc : MonoBehaviour
    {
        [SerializeField] SceneType sceneToLoad;
        [SerializeField] MenuManagerSc menuManager;

        private void Awake()
        {
            if (menuManager == null) Debug.LogError("MenuManager reference is missing in SceneLoadTrigger.");
        }

        public void TriggerLoad()
        {
            if (menuManager != null) menuManager.LoadScene(sceneToLoad);
        }
    }
}