using System;
using UnityEngine;

namespace Mototaxi.Core
{
    public class TimeManagerSc : MonoBehaviour
    {
        public static float ElapsedTime { get; private set; }
        public static event Action<float> OnSecondPassed;

        private float _timeAccumulator = 0f;

        private void Update()
        {
            float delta = Time.deltaTime;
            ElapsedTime += delta;
            _timeAccumulator += delta;

            if (_timeAccumulator >= 1f)
            {
                _timeAccumulator -= 1f;
                OnSecondPassed?.Invoke(ElapsedTime);
            }
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