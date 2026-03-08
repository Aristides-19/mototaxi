using UnityEngine;
using UnityEditor;
using Gley.TrafficSystem;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Editor tool to quickly create a Gley Traffic System VehiclePool ScriptableObject from a set of prefabs.
/// </summary>
public class GleyVehiclePoolCreatorSc : EditorWindow
{
    [SerializeField] GameObject[] sourcePrefabs;
    [SerializeField] string fileName = "MototaxiTrafficPool";
    [SerializeField] string folderPath = "Assets/Scriptables";
    [SerializeField] int defaultPercent = 10;

    [MenuItem("Tools/Mototaxi/Gley Vehicle Pool Creator")]
    public static void ShowWindow()
    {
        GetWindow<GleyVehiclePoolCreatorSc>("Vehicle Pool Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Gley Vehicle Pool Creator", EditorStyles.boldLabel);

        // --- Drop Zone ---
        Event evt = Event.current;
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 60.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "\tDROP PREFABS HERE", EditorStyles.centeredGreyMiniLabel);

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    List<GameObject> droppedPrefabs = new();
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject is GameObject go && PrefabUtility.IsPartOfPrefabAsset(go))
                        {
                            droppedPrefabs.Add(go);
                        }
                    }

                    if (droppedPrefabs.Count > 0)
                    {
                        sourcePrefabs = droppedPrefabs.ToArray();
                    }
                }
                break;
        }
        // -----------------------

        ScriptableObject target = this;
        SerializedObject so = new(target);

        SerializedProperty prefabsProperty = so.FindProperty("sourcePrefabs");
        EditorGUILayout.PropertyField(prefabsProperty, true);

        defaultPercent = EditorGUILayout.IntSlider("Default Percent", defaultPercent, 1, 100);

        so.ApplyModifiedProperties();

        EditorGUILayout.Space();
        GUILayout.Label("Output Settings", EditorStyles.boldLabel);
        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);
        fileName = EditorGUILayout.TextField("File Name", fileName);

        if (GUILayout.Button("Create Vehicle Pool"))
        {
            CreatePool();
        }
    }

    private void CreatePool()
    {
        if (sourcePrefabs == null || sourcePrefabs.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "Please assign at least one source prefab.", "OK");
            return;
        }

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fullPath = Path.Combine(folderPath, fileName + ".asset");

        // Check if file exists to ask for overwrite
        if (File.Exists(fullPath))
        {
            if (!EditorUtility.DisplayDialog("Overwrite?", $"A VehiclePool named {fileName} already exists. Do you want to overwrite it?", "Yes", "No"))
            {
                return;
            }
        }

        VehiclePool pool = CreateInstance<VehiclePool>();

        List<CarType> trafficCars = new();

        foreach (var prefab in sourcePrefabs)
        {
            if (prefab == null) continue;

            CarType carType = new();

            trafficCars.Add(carType);
        }

        pool.trafficCars = trafficCars.ToArray();

        // Use SerializedObject to set the private fields of CarType within the pool
        SerializedObject poolSo = new(pool);
        SerializedProperty carsProp = poolSo.FindProperty("trafficCars");

        int carIndex = 0;
        for (int i = 0; i < sourcePrefabs.Length; i++)
        {
            if (sourcePrefabs[i] == null) continue;

            SerializedProperty element = carsProp.GetArrayElementAtIndex(carIndex);
            element.FindPropertyRelative("vehiclePrefab").objectReferenceValue = sourcePrefabs[i];
            element.FindPropertyRelative("percent").intValue = defaultPercent;
            element.FindPropertyRelative("ignore").boolValue = false;
            carIndex++;
        }

        poolSo.ApplyModifiedProperties();

        AssetDatabase.CreateAsset(pool, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = pool;

        EditorUtility.DisplayDialog("Success", $"Vehicle Pool created at {fullPath}", "OK");
    }
}
