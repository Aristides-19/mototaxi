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
        [SerializeField] private LayerMask _pedestriansLayer = 1 << 13;
        public LayerMask PedestriansLayer => _pedestriansLayer;

        [SerializeField] private RoceData _roceSettings = new();
        public RoceData RoceSettings => _roceSettings;

        [SerializeField] private WheelieData _wheelieSettings = new();
        public WheelieData WheelieSettings => _wheelieSettings;

        [SerializeField] private PassengerData _passengerSettings = new();
        public PassengerData PassengerSettings => _passengerSettings;

        [SerializeField] private SpeedData _speedSettings = new();
        public SpeedData SpeedSettings => _speedSettings;

        [SerializeField] private TripData _tripSettings = new();
        public TripData TripSettings => _tripSettings;
    }

    [Serializable]
    public class TripData
    {
        [Tooltip("Base score for picking up a passenger")]
        [SerializeField] private float _basePickupScore = 50f;
        public float BasePickupScore => _basePickupScore;

        [Tooltip("Base score for successfully delivering a passenger")]
        [SerializeField] private float _baseDropOffScore = 100f;
        public float BaseDropOffScore => _baseDropOffScore;

        [Tooltip("Multiplier for time bonus. Formula: Base * (TimeMultiplier / Duration)")]
        [SerializeField] private float _timeBonusMultiplier = 60f;
        public float TimeBonusMultiplier => _timeBonusMultiplier;

    }

    [Serializable]
    public class SpeedData
    {
        [Tooltip("Percentage of max speed required to start scoring (0.0 to 1.0)")]
        [Range(0f, 1f)]
        [SerializeField] private float _speedThresholdPercentage = 0.975f;
        public float SpeedThresholdPercentage => _speedThresholdPercentage;

        [Tooltip("Score added per second while above threshold")]
        [SerializeField] private float _scoreMultiplier = 7.5f;
        public float ScoreMultiplier => _scoreMultiplier;

        [Tooltip("Interval in seconds to score points after being above threshold (or after sending points again)")]
        [SerializeField] private float _intervalToStartScoring = 0.5f;
        public float IntervalToStartScoring => _intervalToStartScoring;
    }

    [Serializable]
    public class PassengerData
    {
        [Tooltip("How many passengers to spawn when the game starts.")]
        [SerializeField] private int _initialPassengers = 10;
        public int InitialPassengers => _initialPassengers;

        [Tooltip("Maximum number of active passengers allowed in the world.")]
        [SerializeField] private int _maxPassengers = 20;
        public int MaxPassengers => _maxPassengers;

        [Tooltip("Percentage of available spawn points to occupy initially (0 to 1).")]
        [SerializeField, Range(0, 1)] private float _spawnDensity = 0.5f;
        public float SpawnDensity => _spawnDensity;

        [Tooltip("Minimum distance required between spawn and destination points.")]
        [SerializeField] private float _minTripDistance = 150f;
        public float MinTripDistance => _minTripDistance;

        [Tooltip("Units to reduce player max speed per unit of passenger mass (e.g., 0.25 km/h per kg)")]
        [SerializeField] private float _maxKmLossPerMassUnit = 0.35f;
        public float MaxKmLossPerMassUnit => _maxKmLossPerMassUnit / 3.6f;
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