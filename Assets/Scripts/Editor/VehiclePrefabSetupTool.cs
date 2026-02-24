using UnityEngine;
using UnityEditor;
using Gley.TrafficSystem;
// Importamos el namespace que mencionaste en tu c�digo original para las luces
using Gley.UrbanSystem.Internal;

public class VehiclePrefabSetupTool : Editor
{
    [MenuItem("Tools/Setup Vehicle Prefabs in Selected Folder")]
    public static void SetupPrefabs()
    {
        string[] guids = Selection.assetGUIDs;
        if (guids.Length == 0)
        {
            Debug.LogWarning("Por favor, selecciona una carpeta en la ventana de Project primero.");
            return;
        }

        string selectedPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        if (!AssetDatabase.IsValidFolder(selectedPath))
        {
            Debug.LogWarning("La selecci�n no es una carpeta.");
            return;
        }

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { selectedPath });

        if (prefabGuids.Length == 0)
        {
            Debug.LogWarning("No se encontraron prefabs en la carpeta seleccionada.");
            return;
        }

        int processedCount = 0;

        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            // USAMOS SCOPE: Esto abre el prefab, permite editarlo y lo guarda autom�ticamente 
            // al terminar, respetando si es un Prefab Variant.
            using (var editingScope = new PrefabUtility.EditPrefabContentsScope(assetPath))
            {
                GameObject prefabContents = editingScope.prefabContentsRoot;

                // --- 1. A�ADIR U OBTENER COMPONENTES PRINCIPALES ---
                VehicleComponent vehicleComp = prefabContents.GetComponent<VehicleComponent>();
                if (vehicleComp == null) vehicleComp = prefabContents.AddComponent<VehicleComponent>();

                VehicleLightsComponent lightsComp = prefabContents.GetComponent<VehicleLightsComponent>();
                if (lightsComp == null) lightsComp = prefabContents.AddComponent<VehicleLightsComponent>();


                // --- 2. CONFIGURAR RIGIDBODY ---
                Rigidbody rb = prefabContents.GetComponent<Rigidbody>();
                if (rb == null) rb = prefabContents.AddComponent<Rigidbody>();

                rb.mass = 1500f;
                rb.linearDamping = 0.1f; // En el Inspector se muestra como "Linear Damping"
                rb.angularDamping = 3f; // En el Inspector se muestra como "Angular Damping"
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rb.interpolation = RigidbodyInterpolation.None;

                vehicleComp.rb = rb;


                // --- 3. ASIGNAR REFERENCIAS DE VEHICLE COMPONENT ---
                vehicleComp.carHolder = FindOrCreateChild(prefabContents.transform, "CarHolder");
                vehicleComp.frontTrigger = FindOrCreateChild(prefabContents.transform, "FrontTriggerHolder");
                vehicleComp._frontPosition = FindOrCreateChild(prefabContents.transform, "VehicleFrontPosition");
                vehicleComp._backPosition = FindOrCreateChild(prefabContents.transform, "VehicleBackPosition");


                // --- 4. ASIGNAR REFERENCIAS DE VEHICLE LIGHTS COMPONENT ---
                // Basado en la captura, asumo que las variables se llaman as� y son GameObjects.
                // Si el paquete de Gley usa nombres de variables distintos internamente, podr�as 
                // necesitar ajustar "frontLights", "reverseLights", etc., por sus nombres reales.

                // NOTA: Usamos .gameObject porque en tu captura el �cono es un cubo (GameObject), no un Transform.
                lightsComp.frontLights = FindOrCreateChild(prefabContents.transform, "FrontLights").gameObject;
                lightsComp.reverseLights = FindOrCreateChild(prefabContents.transform, "ReverseLights").gameObject;
                lightsComp.rearLights = FindOrCreateChild(prefabContents.transform, "RearLights").gameObject;
                lightsComp.stopLights = FindOrCreateChild(prefabContents.transform, "StopLights").gameObject;
                lightsComp.blinkerLeft = FindOrCreateChild(prefabContents.transform, "BlinkersLeft").gameObject;
                lightsComp.blinkerRight = FindOrCreateChild(prefabContents.transform, "BlinkersRight").gameObject;


                // --- 5. A�ADIR MESH COLLIDERS A LOS HIJOS ---
                MeshFilter[] childrenWithMesh = prefabContents.GetComponentsInChildren<MeshFilter>(true);
                foreach (MeshFilter meshFilter in childrenWithMesh)
                {
                    GameObject child = meshFilter.gameObject;
                    if (!child.TryGetComponent(out MeshCollider meshCol))
                    {
                        meshCol = child.AddComponent<MeshCollider>();
                    }
                    if (!meshCol.convex)
                    {
                        meshCol.convex = true;
                    }
                }

                processedCount++;
            } // Al cerrar las llaves del 'using', Unity guarda el prefab autom�ticamente.
        }

        Debug.Log($"�Proceso terminado! Se configuraron {processedCount} prefabs preservando sus variantes.");
    }

    /// <summary>
    /// Busca un hijo por nombre. Si no existe, crea un GameObject vac�o.
    /// </summary>
    private static Transform FindOrCreateChild( Transform parent, string childName )
    {
        Transform child = parent.Find(childName);
        if (child == null)
        {
            GameObject newChild = new GameObject(childName);
            newChild.transform.SetParent(parent);
            newChild.transform.localPosition = Vector3.zero;
            newChild.transform.localRotation = Quaternion.identity;
            child = newChild.transform;
        }
        return child;
    }
}