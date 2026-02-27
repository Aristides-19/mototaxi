using UnityEngine;
using UnityEditor;

namespace Mototaxi.Editor
{
    public class AddMeshColliderToChildren : EditorWindow
    {
        [MenuItem("Tools/Add Mesh Collider Convex To Children")]
        public static void AddComponent()
        {
            GameObject[] selectedParents = Selection.gameObjects;

            if (selectedParents.Length == 0)
            {
                Debug.LogWarning("Please select a parent object first.");
                return;
            }

            foreach (GameObject parent in selectedParents)
            {
                MeshFilter[] childrenWithMesh = parent.GetComponentsInChildren<MeshFilter>();

                foreach (MeshFilter meshFilter in childrenWithMesh)
                {
                    GameObject child = meshFilter.gameObject;

                    if (child.TryGetComponent(out MeshCollider meshCol) == false)
                    {
                        meshCol = child.AddComponent<MeshCollider>();
                    }

                    meshCol.convex = true;
                }

                Debug.Log($"Component added to the children of {parent.name}");
            }


        }
    }
}