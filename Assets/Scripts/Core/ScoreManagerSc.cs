using System;
using UnityEngine;

namespace Mototaxi.Core
{
    public class ScoreManagerSc : MonoBehaviour
    {
        public static float TotalScore { get; private set; } = 0f;

        public static event Action<float> OnScoreUpdated;
        public static event Action<string, int> OnTrickUpdated;

        public static void AddScore(float amount)
        {
            TotalScore += amount;
            OnScoreUpdated?.Invoke(TotalScore);
        }

        public static void UpdateTrickUI(string trickName, int currentPoints)
        {
            OnTrickUpdated?.Invoke(trickName, currentPoints);
        }
    }
}