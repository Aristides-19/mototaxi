using UnityEngine;
using UnityEditor;
using Gley.TrafficSystem;
using Gley.TrafficSystem.Internal;
using System.Collections.Generic;
using System.IO;
using Gley.TrafficSystem.Editor;

/// <summary>
/// Editor tool to convert existing vehicle prefabs to be compatible with Gley Traffic System. <br/><br/>
/// 
/// NOTE: This assumes the source prefabs have a specific hierarchy structure (Body(with Collider), Wheels/Meshes/FrontLeftWheel, etc.) and will create the necessary structure for Gley VehicleComponent. <br/><br/>
/// Adjust the code as needed for different prefab structures.
/// </summary>
public class GleyTrafficVehiclePortingSc : EditorWindow
{
    [SerializeField] GameObject[] sourcePrefabs;
    [SerializeField] string outputFolder = "Assets/Prefabs/TrafficVehicles";
    [SerializeField] string[] wheelNames = { "FrontLeftWheel", "FrontRightWheel", "RearLeftWheel", "RearRightWheel" };
    [SerializeField] int trafficLayer = 9;


    [MenuItem("Tools/Mototaxi/Gley Traffic Vehicle Porting Tool")]
    public static void ShowWindow()
    {
        GetWindow<GleyTrafficVehiclePortingSc>("Gley Traffic Vehicle Porter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Gley Traffic Vehicle Porting Tool", EditorStyles.boldLabel);

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

        SerializedProperty wheelNamesProperty = so.FindProperty("wheelNames");
        EditorGUILayout.PropertyField(wheelNamesProperty, true);

        SerializedProperty trafficLayerProperty = so.FindProperty("trafficLayer");
        trafficLayerProperty.intValue = EditorGUILayout.LayerField("Traffic Layer", trafficLayerProperty.intValue);

        so.ApplyModifiedProperties();

        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);

        if (GUILayout.Button("Convert Prefabs"))
        {
            ConvertAll();
        }
    }

    private void ConvertAll()
    {
        if (sourcePrefabs == null || sourcePrefabs.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "Please assign at least one source prefab.", "OK");
            return;
        }

        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        foreach (var prefab in sourcePrefabs)
        {
            if (prefab == null) continue;
            ConvertPrefab(prefab);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", "Prefabs converted successfully!", "OK");
    }

    private void ConvertPrefab(GameObject source)
    {
        // Instantiate temporary instance of the source prefab
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(source);
        PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        instance.name = source.name + "_Gley";

        // Setup rigidbody properties
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        if (rb == null) rb = instance.AddComponent<Rigidbody>();
        rb.mass = 1500;
        rb.angularDamping = 3;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Setup VehicleComponent
        VehicleComponent vc = instance.GetComponent<VehicleComponent>();
        if (vc == null) vc = instance.AddComponent<VehicleComponent>();

        // Create CarHolder
        GameObject carHolderObj = new("CarHolder");
        carHolderObj.transform.SetParent(instance.transform);
        carHolderObj.transform.localPosition = Vector3.zero;

        // Game Cars EXACT STRUCTURE
        Transform wheelsRoot = instance.transform.Find("Wheels");
        Transform meshesContainer = wheelsRoot?.Find("Meshes");

        if (wheelsRoot == null || meshesContainer == null)
        {
            Debug.LogError($"Could not find 'Wheels/Meshes' hierarchy in {source.name}. Check hierarchy!");
            DestroyImmediate(instance);
            return;
        }

        // Reparent just in case
        wheelsRoot.SetParent(carHolderObj.transform);

        // Reparent Body WITH Collider
        Transform bodyMesh = instance.transform.Find("Body");
        if (bodyMesh == null) bodyMesh = wheelsRoot.Find("Body");
        if (bodyMesh != null) bodyMesh.SetParent(carHolderObj.transform);

        // Wheels for Gley: WheelPivot -> WheelGraphics -> OriginalMesh
        GameObject gleyWheelsContainer = new("Wheels");
        gleyWheelsContainer.transform.SetParent(carHolderObj.transform);
        gleyWheelsContainer.transform.localPosition = Vector3.zero;

        List<Wheel> gleyWheelsList = new();

        foreach (string wName in wheelNames)
        {
            Transform originalWheelMesh = meshesContainer.Find(wName);
            if (originalWheelMesh == null) continue;

            // Calculate wheel radius based on mesh bounds (fallback to 0.33 if not found)
            float radius = 0.33f;
            MeshFilter mf = originalWheelMesh.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
            {
                radius = mf.sharedMesh.bounds.extents.y * originalWheelMesh.lossyScale.y;
            }

            // Create Gley wheel structure: WheelPivot -> WheelGraphics -> OriginalMesh
            GameObject firstPivot = new(wName.Replace("Wheel", ""));
            firstPivot.transform.SetParent(gleyWheelsContainer.transform);
            firstPivot.transform.position = Vector3.Scale(originalWheelMesh.position, new Vector3(1, 0, 1));

            GameObject secondPivot = new("VehicleWheelRadiusPivot");
            secondPivot.transform.SetParent(firstPivot.transform);
            secondPivot.transform.localPosition = new(0, radius, 0);

            // Move original wheel mesh under second pivot
            originalWheelMesh.SetParent(secondPivot.transform);
            originalWheelMesh.localPosition = Vector3.zero;

            Wheel gWheel = new()
            {
                wheelTransform = firstPivot.transform,
                wheelGraphics = secondPivot.transform,
                wheelRadius = radius,
                wheelPosition = wName.Contains("Front") ? Wheel.WheelPosition.Front : Wheel.WheelPosition.Back
            };

            gleyWheelsList.Add(gWheel);
        }

        DestroyImmediate(wheelsRoot.gameObject);

        SetLayerRecursively(instance, trafficLayer);

        // Own settings for game
        vc.rb = rb;
        vc.carHolder = carHolderObj.transform;
        vc.allWheels = gleyWheelsList.ToArray();
        vc.maxSteer = 45;
        vc.minPossibleSpeed = 30;
        vc.maxPossibleSpeed = 110;
        vc.accelerationTime = 2;
        vc.brakeTime = 2;
        vc.distanceToStop = 2;
        vc.triggerLength = 2;

        // Let GleyVehicleComponentEditor configure the rest of the settings based on the assigned values
        VehicleComponentEditor.ConfigureCar(vc);

        // Save as new prefab
        string finalPath = Path.Combine(outputFolder, instance.name + ".prefab");
        PrefabUtility.SaveAsPrefabAsset(instance, finalPath);

        // Destroy temporary instance
        DestroyImmediate(instance);
        Debug.Log($"[GleyTrafficPorter] Success: {source.name} -> {finalPath}");
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
