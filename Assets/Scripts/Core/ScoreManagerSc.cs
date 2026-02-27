using System;

namespace Mototaxi.Core
{
    public static class ScoreManagerSc
    {
        public static float CurrentScore { get; private set; } = 0f;

        public static event Action<float> OnScoreChanged;

        public static void AddScore(float score)
        {
            CurrentScore += score;
            OnScoreChanged?.Invoke(CurrentScore);
        }

        public static void ResetScore()
        {
            CurrentScore = 0f;
            OnScoreChanged?.Invoke(CurrentScore);
        }
    }
}