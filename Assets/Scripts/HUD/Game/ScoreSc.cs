using System;
using Mototaxi.Core;
using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    class ScoreSc : MonoBehaviour
    {
        #region Inspector Settings
        [Header("Score Display")]
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TMPCanvasSc scoreChangeText;
        [SerializeField] TMPCanvasSc scoreSourceText;

        [Header("Opacity Settings")]
        [SerializeField] float fadeDuration = 0.5f;
        [SerializeField] float displayDuration = 1f;
        [SerializeField] float maxAlpha = 1f;

        [Header("Punch Scale Settings")]
        [SerializeField] float punchScale = 1.3f;
        [SerializeField] float punchDuration = 0.1f;
        #endregion

        #region Init Settings
        private void Awake()
        {
            if (scoreText == null) Debug.LogError("Score TextMeshProUGUI reference is missing in ScoreSc.");

            // Score Change
            originalScoreChangeScale = scoreChangeText.transform.localScale;
            UtilsSc.SetCanvasAlpha(scoreChangeText.canvasGroup, 0f);

            // Score Source
            UtilsSc.SetCanvasAlpha(scoreSourceText.canvasGroup, 0f);

            // Init score display with current score just in case
            HandleScore(ScoreManagerSc.CurrentScore, 0f, ScoreSource.None);

        }
        private void OnEnable() => ScoreManagerSc.OnScoreChanged += HandleScore;
        private void OnDisable() => ScoreManagerSc.OnScoreChanged -= HandleScore;
        #endregion

        #region Score Handling
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
        #endregion

        #region Score Source
        private Coroutine fadeScoreSourceCoroutine;
        private void AddScoreSource(float change, ScoreSource source)
        {
            if (change <= 0) return;

            float roundedChange = MathF.Round(change, 2);

            scoreSourceText.tmp.text = source switch
            {
                ScoreSource.Roce => $"+{roundedChange} ROCE",
                ScoreSource.Wheelie => $"+{roundedChange} CABALLITO",
                _ => ""
            };

            UtilsSc.SetCanvasAlpha(scoreSourceText.canvasGroup, maxAlpha);

            if (fadeScoreSourceCoroutine != null) StopCoroutine(fadeScoreSourceCoroutine);
            fadeScoreSourceCoroutine = StartCoroutine(UtilsSc.FadeRoutine(displayDuration, fadeDuration, maxAlpha, scoreSourceText.canvasGroup, () => fadeScoreSourceCoroutine = null));
        }
        #endregion

        #region Score Change
        private Vector3 originalScoreChangeScale;
        private float currentScoreChange;
        private Coroutine fadeScoreChangeCoroutine;
        private Coroutine punchScoreChangeCoroutine;
        private void AddScoreChange(float change)
        {
            if (change <= 0) return;

            currentScoreChange += change;
            scoreChangeText.tmp.text = $"+{MathF.Round(currentScoreChange, 2)}";

            UtilsSc.SetCanvasAlpha(scoreChangeText.canvasGroup, maxAlpha);

            if (fadeScoreChangeCoroutine != null) StopCoroutine(fadeScoreChangeCoroutine);
            fadeScoreChangeCoroutine = StartCoroutine(UtilsSc.FadeRoutine(displayDuration, fadeDuration, maxAlpha, scoreChangeText.canvasGroup, () =>
            {
                currentScoreChange = 0;
                fadeScoreChangeCoroutine = null;
            }));

            if (punchScoreChangeCoroutine != null) StopCoroutine(punchScoreChangeCoroutine);
            punchScoreChangeCoroutine = StartCoroutine(UtilsSc.PunchScaleRoutine(originalScoreChangeScale, punchScale, punchDuration, scoreChangeText.transform, () => punchScoreChangeCoroutine = null));
        }
        #endregion
    }
}