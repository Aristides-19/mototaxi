using System;
using System.Collections;
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

            UpdateScoreChangeTextAlpha(0f);
            HandleScore(Core.ScoreManagerSc.CurrentScore, 0f);

        }
        private void OnEnable()
        {
            Core.ScoreManagerSc.OnScoreChanged += HandleScore;
        }

        private void OnDisable()
        {
            Core.ScoreManagerSc.OnScoreChanged -= HandleScore;
        }

        private void HandleScore(float score, float change)
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

            UpdateScoreChangeTextAlpha(maxAlpha);

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(ScoreFadeRoutine());

            if (punchCoroutine != null) StopCoroutine(punchCoroutine);
            punchCoroutine = StartCoroutine(PunchScaleRoutine());
        }

        private IEnumerator PunchScaleRoutine()
        {
            float elapsed = 0f;
            Vector3 targetScale = originalScale * punchScale;

            while (elapsed < punchDuration)
            {
                elapsed += Time.deltaTime;
                scoreChangeText.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / punchDuration);
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < punchDuration)
            {
                elapsed += Time.deltaTime;
                scoreChangeText.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / punchDuration);
                yield return null;
            }

            scoreChangeText.transform.localScale = originalScale;
            punchCoroutine = null;
        }

        private IEnumerator ScoreFadeRoutine()
        {
            yield return new WaitForSeconds(displayDuration);

            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float newAlpha = Mathf.Lerp(maxAlpha, 0f, elapsed / fadeDuration);

                UpdateScoreChangeTextAlpha(newAlpha);
                yield return null;
            }

            currentScoreChange = 0;
            fadeCoroutine = null;
        }

        private void UpdateScoreChangeTextAlpha(float alpha)
        {
            Color color = scoreChangeText.color;
            scoreChangeText.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}