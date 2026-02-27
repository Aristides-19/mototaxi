using System;
using UnityEngine;

namespace Mototaxi.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Mototaxi/GameData", order = 0)]
    public class GameDataSc : ScriptableObject
    {
        public LayerMask TrafficLayer = 1 << 9;

        public RoceData RoceSettings = new();
    }

    [Serializable]
    public class RoceData
    {
        [Tooltip("Base score multiplier for a roce, it depends on RoceMechanic to apply it correctly")]
        public float ScoreMultiplier = 1.1f;
        [Tooltip("Maximum distance for a roce to be registered in meters")]
        public float DistanceThreshold = 1.5f;
        [Tooltip("Minimum velocity required to register a roce in m/s")]
        public float MinVelocity = 4f;
    }
}