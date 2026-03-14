using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
#endif

namespace Mototaxi.Core
{
    [CreateAssetMenu(fileName = "ScenesData", menuName = "Mototaxi/Core/ScenesData", order = 1)]
    public class ScenesDataSO : ScriptableObject
    {
        [SerializedDictionary("Scene Type", "Build Index")]
        [SerializeField] private SerializedDictionary<SceneType, int> SceneMap;

        public int GetBuildIndex(SceneType type)
        {
            if (SceneMap.TryGetValue(type, out int index)) return index;

            Debug.LogError($"[SceneData] The SceneType '{type}' is not assigned in the Dictionary.");
            return -1;
        }

#if UNITY_EDITOR
        [ContextMenu("Update Indices By Name")]
        public void UpdateIndicesBySceneName()
        {
            var keys = new List<SceneType>(SceneMap.Keys);

            foreach (var type in keys)
            {
                string nameToSearch = type.ToString();
                int realIndex = -1;

                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    string path = SceneUtility.GetScenePathByBuildIndex(i);
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

                    if (string.Equals(fileName, nameToSearch, StringComparison.OrdinalIgnoreCase))
                    {
                        realIndex = i;
                        break;
                    }
                }

                if (realIndex != -1)
                {
                    SceneMap[type] = realIndex;
                    Debug.Log($"<color=green>[SceneData] Updated {type}: Index {realIndex}</color>");
                }
                else
                {
                    Debug.LogError($"<color=red>[SceneData] Scene '{nameToSearch}' not found in Build Settings!</color>");
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }

    public enum SceneType { Menu, Road }
}