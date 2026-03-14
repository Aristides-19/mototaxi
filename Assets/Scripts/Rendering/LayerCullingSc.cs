using System;
using Mototaxi.Core;
using Mototaxi.Utils;
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
            distances[FunctionsSc.GetLayerFromLayerMask(gameData.GroundLayer)] = GroundDistance;
            distances[4] = LayerDefaultDistance;
            distances[5] = LayerDefaultDistance;
            distances[FunctionsSc.GetLayerFromLayerMask(gameData.MountainLayer)] = MountainDistance;
            distances[FunctionsSc.GetLayerFromLayerMask(gameData.RoadLayer)] = RoadDistance;
            distances[FunctionsSc.GetLayerFromLayerMask(gameData.TrafficLayer)] = TrafficDistance;
            distances[FunctionsSc.GetLayerFromLayerMask(gameData.BuildingsLayer)] = BuildingsDistance;
            distances[FunctionsSc.GetLayerFromLayerMask(gameData.ObstaclesLayer)] = ObstaclesDistance;
            distances[FunctionsSc.GetLayerFromLayerMask(gameData.PlayerLayer)] = PlayerDistance;

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