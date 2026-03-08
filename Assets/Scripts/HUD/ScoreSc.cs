using System;
using System.Collections;
using Mototaxi.Core;
using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    class ScoreSc : MonoBehaviour
    {
        [Header("Score Display")]
        [SerializeField] TextMeshProUGUI scoreText;

        [Header("Opacity Settings")]
        [SerializeField] TextMeshProUGUI scoreChangeText;
        [SerializeField] float fadeDuration = 0.5f;
        [SerializeField] float displayDuration = 1f;
        [SerializeField] float maxAlpha = 1f;

        [Header("Punch Scale Settings")]
        [SerializeField] float punchScale = 1.3f;
        [SerializeField] float punchDuration = 0.1f;

        private Vector3 originalScale;
        private float currentScoreChange;
        private Coroutine fadeCoroutine;
        private Coroutine punchCoroutine;

        private void Awake()
        {
            if (scoreText == null)
            {
                Debug.LogError("Score TextMeshProUGUI reference is missing in ScoreSc.");
            }

            originalScale = scoreChangeText.transform.localScale;

            TMPUtilsSc.SetTextAlpha(scoreChangeText, 0f);
            HandleScore(ScoreManagerSc.CurrentScore, 0f, ScoreSource.None);

        }
        private void OnEnable()
        {
            ScoreManagerSc.OnScoreChanged += HandleScore;
        }

        private void OnDisable()
        {
            ScoreManagerSc.OnScoreChanged -= HandleScore;
        }

        private void HandleScore(float score, float change, ScoreSource source)
        {
            UpdateScore(score);
            AddScoreChange(change);
        }

        private void UpdateScore(float score)
        {
            scoreText.text = $"Bs. <size=+20>{MathF.Round(score, 2)}</size>";
        }

        private void AddScoreChange(float change)
        {
            if (change <= 0) return;

            currentScoreChange += change;
            scoreChangeText.text = $"+{MathF.Round(currentScoreChange, 2)}";

            TMPUtilsSc.SetTextAlpha(scoreChangeText, maxAlpha);

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(TMPUtilsSc.FadeRoutine(displayDuration, fadeDuration, maxAlpha, scoreChangeText, () =>
            {
                currentScoreChange = 0;
                fadeCoroutine = null;
            }));

            if (punchCoroutine != null) StopCoroutine(punchCoroutine);
            punchCoroutine = StartCoroutine(TMPUtilsSc.PunchScaleRoutine(originalScale, punchScale, punchDuration, scoreChangeText, () => punchCoroutine = null));
        }
    }
}