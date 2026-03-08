using System;
using UnityEngine;

namespace Mototaxi.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Mototaxi/Core/GameData", order = 0)]
    public class GameDataSc : ScriptableObject
    {
        public LayerMask GroundLayer = 1 << 3;
        public LayerMask MountainLayer = 1 << 7;
        public LayerMask RoadLayer = 1 << 8;
        public LayerMask TrafficLayer = 1 << 9;
        public LayerMask BuildingsLayer = 1 << 10;
        public LayerMask ObstaclesLayer = 1 << 11;
        public LayerMask PlayerLayer = 1 << 12;

        public RoceData RoceSettings = new();
        // Añadimos la configuración del wheelie
        public WheelieData WheelieSettings = new();
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

    // NUEVA ESTRUCTURA PARA EL WHEELIE
    [Serializable]
    public class WheelieData
    {
        [Tooltip("Puntos otorgados por cada segundo en wheelie")]
        public float PointsPerSecond = 10f;
        [Tooltip("Velocidad mínima requerida para registrar el wheelie en m/s")]
        public float MinVelocity = 5f;
        [Tooltip("Ángulo mínimo de inclinación para empezar a sumar puntos (ej. 20)")]
        public float MinInclineAngle = 20f;
    }
}