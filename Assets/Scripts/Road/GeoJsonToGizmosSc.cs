using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class GeoJsonToGizmosSc : MonoBehaviour
{
    [Header("GEOJson Source")]
    public TextAsset geoJsonFile;

    [Header("Gizmos Settings")]
    public bool showNodes = true;
    public Color pathColor = Color.red;
    [Range(0.1f, 5f)] public float nodeSize = 3f;

    [Header("Nodes")]
    public Dictionary<string, List<Vector3>> unityFeatureNodes = new();

    private const double metersPerDegree = 111111f;

    private double metersPerLat;
    private double metersPerLon;

    private double refLon;
    private double refLat;

    private void OnValidate()
    {
        if (geoJsonFile != null)
        {
            ParseGeoJson();
        }
    }

    [ContextMenu("Parse GeoJson")]
    private void ParseGeoJson()
    {

        GeoJsonData data = JsonConvert.DeserializeObject<GeoJsonData>(geoJsonFile.text);
        unityFeatureNodes.Clear();

        if (data.features == null || data.features.Count == 0) return;

        var firstCoords = data.features[0].geometry.coordinates[0];
        refLon = firstCoords[0];
        refLat = firstCoords[1];

        metersPerLat = metersPerDegree;
        metersPerLon = metersPerDegree * Math.Cos(refLat * Math.PI / 180.0);

        foreach (var feature in data.features)
        {
            if (feature.geometry.type == "LineString")
            {
                List<Vector3> worldPoints = new();
                foreach (var coord in feature.geometry.coordinates)
                {
                    worldPoints.Add(ConvertCoords(coord[0], coord[1]));
                }

                unityFeatureNodes.Add(feature.properties.name, worldPoints);
                float distance = CalculateLineDistance(worldPoints);

                Debug.Log($"Parsed LineString: {feature.properties.name}, Distance: {distance:F2} meters, Nodes: {worldPoints.Count}");
            }
        }

        Debug.Log($"Total nodes parsed: {unityFeatureNodes.Count}");
    }

    private float CalculateLineDistance(List<Vector3> points)
    {
        float totalDistance = 0f;

        for (int i = 1; i < points.Count; i++)
        {
            float d = Vector3.Distance(points[i - 1], points[i]);
            totalDistance += d;
        }

        return totalDistance;
    }

    private Vector3 ConvertCoords(double lon, double lat)
    {
        float x = (float)((lon - refLon) * metersPerLon);
        float z = (float)((lat - refLat) * metersPerLat);
        return new Vector3(x, 0, z);
    }

    private void OnDrawGizmos()
    {
        if (!showNodes || unityFeatureNodes == null || unityFeatureNodes.Count == 0) return;

        Gizmos.color = pathColor;

        foreach (var feature in unityFeatureNodes)
        {
            List<Vector3> points = feature.Value;

            for (int i = 0; i < points.Count; i++)
            {
                Vector3 worldPos = transform.TransformPoint(points[i]);

                Gizmos.DrawSphere(worldPos, nodeSize);

                if (i < points.Count - 1)
                {
                    Vector3 nextWorldPos = transform.TransformPoint(points[i + 1]);
                    Gizmos.DrawLine(worldPos, nextWorldPos);
                }
            }
        }
    }
}

[Serializable]
public class GeoJsonData
{
    public List<Feature> features;
}

[Serializable]
public class Feature
{
    public Properties properties;
    public Geometry geometry;
}

[Serializable]
public class Properties
{
    public string name;
}

[Serializable]
public class Geometry
{
    public string type;
    public List<List<double>> coordinates;
}