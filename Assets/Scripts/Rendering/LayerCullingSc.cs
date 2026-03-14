using System;
using Mototaxi.Core;
using UnityEngine;

namespace Mototaxi.Rendering
{
    [RequireComponent(typeof(Camera))]
    public class LayerCullingSc : MonoBehaviour
    {
        [SerializeField] GameDataSO gameData;

        [Header("Layer Distances")]
        [SerializeField] float GroundDistance = 200f;
        [SerializeField] float MountainDistance = 1000f;
        [SerializeField] float RoadDistance = 200f;
        [SerializeField] float TrafficDistance = 200f;
        [SerializeField] float BuildingsDistance = 200f;
        [SerializeField] float ObstaclesDistance = 100f;
        [SerializeField] float PlayerDistance = 200f;
        [SerializeField] float LayerDefaultDistance = 200f;

        private void Awake()
        {
            ApplyCulling();
        }

        private void ApplyCulling()
        {

            Camera cam = GetComponent<Camera>();
            float[] distances = new float[32];

            distances[0] = LayerDefaultDistance;
            distances[1] = LayerDefaultDistance;
            distances[2] = LayerDefaultDistance;
            distances[(int)Math.Log(gameData.GroundLayer.value, 2)] = GroundDistance;
            distances[4] = LayerDefaultDistance;
            distances[5] = LayerDefaultDistance;
            distances[(int)Math.Log(gameData.MountainLayer.value, 2)] = MountainDistance;
            distances[(int)Math.Log(gameData.RoadLayer.value, 2)] = RoadDistance;
            distances[(int)Math.Log(gameData.TrafficLayer.value, 2)] = TrafficDistance;
            distances[(int)Math.Log(gameData.BuildingsLayer.value, 2)] = BuildingsDistance;
            distances[(int)Math.Log(gameData.ObstaclesLayer.value, 2)] = ObstaclesDistance;
            distances[(int)Math.Log(gameData.PlayerLayer.value, 2)] = PlayerDistance;

            cam.layerCullDistances = distances;
        }

        private void OnValidate()
        {
            if (Application.isPlaying && GetComponent<Camera>() != null)
            {
                ApplyCulling();
            }
        }
    }
}