using System;

namespace Mototaxi.Core
{
    public static class ScoreManagerSc
    {
        public static float CurrentScore { get; private set; } = 0f;

        /// <summary>
        /// Event triggered when the score changes. Provides the new score and the change amount.
        /// </summary>
        public static event Action<float, float> OnScoreChanged;

        public static void AddScore(float change)
        {
            CurrentScore += change;
            OnScoreChanged?.Invoke(CurrentScore, change);
        }

        public static void ResetScore()
        {
            float lastScore = CurrentScore;
            CurrentScore = 0f;
            OnScoreChanged?.Invoke(CurrentScore, -lastScore);
        }
    }
}