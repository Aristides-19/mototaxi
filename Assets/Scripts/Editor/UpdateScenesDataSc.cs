using UnityEngine;
using UnityEditor;
using Mototaxi.Core;

namespace Mototaxi.Editor
{

    [CustomEditor(typeof(ScenesDataSO))]
    public class SceneDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ScenesDataSO script = (ScenesDataSO)target;

            GUILayout.Space(15);
            GUI.backgroundColor = Color.cyan;

            if (GUILayout.Button("Sync Indices with Build Settings", GUILayout.Height(30)))
            {
                script.UpdateIndicesBySceneName();
            }

            GUI.backgroundColor = Color.white;
        }
    }
}