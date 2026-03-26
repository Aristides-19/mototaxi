using ArcadeBP_Pro;
using Mototaxi.Core;
using Mototaxi.Player;
using UnityEngine;

namespace Mototaxi.Mechanics
{
    [RequireComponent(typeof(ArcadeBikeControllerPro))]
    public class SpeedMechanicSc : MonoBehaviour
    {
        [SerializeField] private GameDataSO _gameData;

        private ArcadeBikeControllerPro _bikeController;
        private float speedTimer;

        private void Awake()
        {
            _bikeController = GetComponent<ArcadeBikeControllerPro>();
            if (_gameData == null) Debug.LogError("GameData reference is missing in SpeedMechanicSc.");
        }

        private void OnEnable() => ArcadeBikeControllerPro.OnLocalVelocityChange += OnVelocityChange;

        private void OnDisable() => ArcadeBikeControllerPro.OnLocalVelocityChange -= OnVelocityChange;


        private void OnVelocityChange(Vector3 velocity)
        {
            if (!PlayerStateSc.IsOnTrip) return;

            float currentSpeed = velocity.magnitude;
            float maxSpeed = _bikeController.bikeSettings.maxSpeed;
            float threshold = maxSpeed * _gameData.SpeedSettings.SpeedThresholdPercentage;

            if (currentSpeed >= threshold)
            {
                speedTimer += Time.deltaTime;
                if (speedTimer >= _gameData.SpeedSettings.IntervalToStartScoring)
                {
                    ScoreManagerSc.AddScore(_gameData.SpeedSettings.ScoreMultiplier * Time.deltaTime, ScoreSource.MaxSpeed);
                    speedTimer = 0f;
                }
            }
            else
            {
                speedTimer = 0f;
            }

        }
    }
}