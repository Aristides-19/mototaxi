using System;

namespace Mototaxi.Core
{
    public static class ScoreManagerSc
    {
        public static float CurrentScore { get; private set; } = 0f;

        /// <summary>
        /// Event triggered when the score changes. Provides the new score and the change amount.
        /// </summary>
        public static event Action<float, float, ScoreSource> OnScoreChanged;

        public static void AddScore(float change, ScoreSource source)
        {
            CurrentScore += change;
            OnScoreChanged?.Invoke(CurrentScore, change, source);
        }

        public static void ResetScore()
        {
            float lastScore = CurrentScore;
            CurrentScore = 0f;
            OnScoreChanged?.Invoke(CurrentScore, -lastScore, ScoreSource.None);
        }
    }

    public enum ScoreSource
    {
        Roce,
        Wheelie,
        None
    }
}