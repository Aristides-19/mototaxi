using System;
using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    class ScoreSc : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreText;

        private void Awake()
        {
            if (scoreText == null)
            {
                Debug.LogError("Score TextMeshProUGUI reference is missing in ScoreSc.");
            }
        }
        private void OnEnable()
        {
            Core.ScoreManagerSc.OnScoreChanged += UpdateScore;
        }

        private void OnDisable()
        {
            Core.ScoreManagerSc.OnScoreChanged -= UpdateScore;
        }

        private void UpdateScore(float score)
        {
            scoreText.text = "Bs. " + MathF.Round(score, 2).ToString();
        }
    }
}