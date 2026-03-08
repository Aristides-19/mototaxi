using System;
using Mototaxi.Core;
using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    class ScoreSc : MonoBehaviour
    {
        [Header("Score Display")]
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI scoreChangeText;
        [SerializeField] TextMeshProUGUI scoreSourceText;

        [Header("Opacity Settings")]
        [SerializeField] float fadeDuration = 0.5f;
        [SerializeField] float displayDuration = 1f;
        [SerializeField] float maxAlpha = 1f;

        [Header("Punch Scale Settings")]
        [SerializeField] float punchScale = 1.3f;
        [SerializeField] float punchDuration = 0.1f;

        // ANCHOR: Score Change Related
        private Vector3 originalScoreChangeScale;
        private float currentScoreChange;
        private Coroutine fadeScoreChangeCoroutine;
        private Coroutine punchScoreChangeCoroutine;

        // ANCHOR: Score Source Related
        private Coroutine fadeScoreSourceCoroutine;

        private void Awake()
        {
            if (scoreText == null)
            {
                Debug.LogError("Score TextMeshProUGUI reference is missing in ScoreSc.");
            }

            originalScoreChangeScale = scoreChangeText.transform.localScale;

            TMPUtilsSc.SetTextAlpha(scoreChangeText, 0f);
            TMPUtilsSc.SetTextAlpha(scoreSourceText, 0f);
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
            // NOTE: It doesn't handle negative score changes
            AddScoreChange(change);
            AddScoreSource(change, source);
        }

        private void UpdateScore(float score)
        {
            scoreText.text = $"Bs. <size=+20>{MathF.Round(score, 2)}</size>";
        }

        private void AddScoreSource(float change, ScoreSource source)
        {
            if (change <= 0) return;

            float roundedChange = MathF.Round(change, 2);

            scoreSourceText.text = source switch
            {
                ScoreSource.Roce => $"+{roundedChange} ROCE",
                ScoreSource.Wheelie => $"+{roundedChange} CABALLITO",
                _ => ""
            };

            TMPUtilsSc.SetTextAlpha(scoreSourceText, maxAlpha);

            if (fadeScoreSourceCoroutine != null) StopCoroutine(fadeScoreSourceCoroutine);
            fadeScoreSourceCoroutine = StartCoroutine(TMPUtilsSc.FadeRoutine(displayDuration, fadeDuration, maxAlpha, scoreSourceText, () => fadeScoreSourceCoroutine = null));
        }

        private void AddScoreChange(float change)
        {
            if (change <= 0) return;

            currentScoreChange += change;
            scoreChangeText.text = $"+{MathF.Round(currentScoreChange, 2)}";

            TMPUtilsSc.SetTextAlpha(scoreChangeText, maxAlpha);

            if (fadeScoreChangeCoroutine != null) StopCoroutine(fadeScoreChangeCoroutine);
            fadeScoreChangeCoroutine = StartCoroutine(TMPUtilsSc.FadeRoutine(displayDuration, fadeDuration, maxAlpha, scoreChangeText, () =>
            {
                currentScoreChange = 0;
                fadeScoreChangeCoroutine = null;
            }));

            if (punchScoreChangeCoroutine != null) StopCoroutine(punchScoreChangeCoroutine);
            punchScoreChangeCoroutine = StartCoroutine(TMPUtilsSc.PunchScaleRoutine(originalScoreChangeScale, punchScale, punchDuration, scoreChangeText, () => punchScoreChangeCoroutine = null));
        }
    }
}