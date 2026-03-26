using System;
using UnityEngine;

namespace Mototaxi.Core
{
    public class TimeManagerSc : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private GameDataSO gameData;

        public static float ElapsedTime { get; private set; }
        public static float TripStartTime { get; private set; }
        public static event Action OnTimeUp;

        public static float CurrentTripDuration => Time.time - TripStartTime;
        public static event Action<float> OnSecondPassed;

        private float _timeAccumulator = 0f;
        private bool _isTimeUp = false;

        public static void MarkTripStart()
        {
            TripStartTime = Time.time;
        }

        private void Update()
        {
            if (_isTimeUp) return;

            float delta = Time.deltaTime;
            ElapsedTime += delta;
            _timeAccumulator += delta;

            if (_timeAccumulator >= 1f)
            {
                _timeAccumulator -= 1f;
                OnSecondPassed?.Invoke(ElapsedTime);
            }

            if (ElapsedTime >= gameData.TripSettings.MaxGameDuration)
            {
                HandleTimeUp();
            }
        }

        private void HandleTimeUp()
        {
            _isTimeUp = true;
            OnTimeUp?.Invoke();
        }

        public static void ResetTimerStatic()
        {
            ElapsedTime = 0f;
        }

        private void OnDestroy()
        {
            OnSecondPassed = null;
            ElapsedTime = 0f;
        }
    }
}