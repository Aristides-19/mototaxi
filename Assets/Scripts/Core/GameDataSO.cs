using System;
using UnityEngine;

namespace Mototaxi.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Mototaxi/Core/GameData", order = 0)]
    public class GameDataSO : ScriptableObject
    {
        [SerializeField] private LayerMask _groundLayer = 1 << 3;
        public LayerMask GroundLayer => _groundLayer;
        [SerializeField] private LayerMask _mountainLayer = 1 << 7;
        public LayerMask MountainLayer => _mountainLayer;
        [SerializeField] private LayerMask _roadLayer = 1 << 8;
        public LayerMask RoadLayer => _roadLayer;
        [SerializeField] private LayerMask _trafficLayer = 1 << 9;
        public LayerMask TrafficLayer => _trafficLayer;
        [SerializeField] private LayerMask _buildingsLayer = 1 << 10;
        public LayerMask BuildingsLayer => _buildingsLayer;
        [SerializeField] private LayerMask _obstaclesLayer = 1 << 11;
        public LayerMask ObstaclesLayer => _obstaclesLayer;
        [SerializeField] private LayerMask _playerLayer = 1 << 12;
        public LayerMask PlayerLayer => _playerLayer;

        [SerializeField] private RoceData _roceSettings = new();
        public RoceData RoceSettings => _roceSettings;

        [SerializeField] private WheelieData _wheelieSettings = new();
        public WheelieData WheelieSettings => _wheelieSettings;
    }

    [Serializable]
    public class RoceData
    {
        [Tooltip("Base score multiplier for a roce, it depends on RoceMechanic to apply it correctly")]
        [SerializeField] private float _scoreMultiplier = 1.1f;
        public float ScoreMultiplier => _scoreMultiplier;
        [Tooltip("Maximum distance for a roce to be registered in meters")]
        [SerializeField] private float _distanceThreshold = 1.5f;
        public float DistanceThreshold => _distanceThreshold;
        [Tooltip("Minimum velocity required to register a roce in m/s")]
        [SerializeField] private float _minVelocity = 4f;
        public float MinVelocity => _minVelocity;
    }


    [Serializable]
    public class WheelieData
    {
        [Tooltip("Points per second while performing a wheelie")]
        [SerializeField] private float _pointsPerSecond = 10f;
        public float PointsPerSecond => _pointsPerSecond;
        [Tooltip("Minimum velocity required to register a wheelie in m/s")]
        [SerializeField] private float _minVelocity = 5f;
        public float MinVelocity => _minVelocity;
        [Tooltip("Minimum incline angle to start earning points (e.g., 20°)")]
        [SerializeField] private float _minInclineAngle = 20f;
        public float MinInclineAngle => _minInclineAngle;
        [Tooltip("Interval in seconds to score points after starting a wheelie (or after sending points again)")]
        [SerializeField] private float _intervalToStartScoring = 0.5f;
        public float IntervalToStartScoring => _intervalToStartScoring;
    }
}