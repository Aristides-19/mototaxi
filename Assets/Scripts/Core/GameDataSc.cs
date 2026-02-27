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
        public float ScoreMultiplier = 1.1f;
        public float DistanceThreshold = 1.5f;
    }
}