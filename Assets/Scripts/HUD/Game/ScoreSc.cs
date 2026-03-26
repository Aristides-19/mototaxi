using System;
using System.Collections;
using System.Collections.Generic;
using Mototaxi.Utils;
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

        [Header("Score Source Scroll")]
        [Tooltip("Container that will hold the score source items. It must configure itself with height and width according to prefab and visible sources.")]
        [SerializeField] MaskContainerSc scoreSourceContainer;
        [Tooltip("Prefab for score source items. It will be instantiated for each score source, and used for measure height movement.")]
        [SerializeField] TMPCanvasSc scoreSourcePrefab;
        [SerializeField] float scrollDuration = 0.2f;
        [SerializeField] int maxVisibleSources = 3;

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
            itemHeight = scoreSourcePrefab.rectTransform.sizeDelta.y;
            UtilsSc.SetCanvasAlpha(scoreSourceContainer.canvasGroup, 0f);

            // Init Pool
            sourcePool = new ObjectPoolSc<TMPCanvasSc>(scoreSourcePrefab, scoreSourceContainer.rectTransform, maxVisibleSources, maxVisibleSources + 2);

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
            int integerPart = (int)MathF.Truncate(score);
            int decimalPart = (int)MathF.Round((score - integerPart) * 100);

            string formattedDecimal = decimalPart > 0 ? $"<size=+10>.{decimalPart:D2}</size>" : "";

            scoreText.text = $"Bs. <size=+20>{integerPart}</size>{formattedDecimal}";
        }
        #endregion

        #region Score Source
        private Coroutine fadeScoreSourceCoroutine;
        private Coroutine scrollScoreSourceCoroutine;
        private readonly List<TMPCanvasSc> activeSources = new();
        private ObjectPoolSc<TMPCanvasSc> sourcePool;
        private float itemHeight;
        private ScoreSource lastSourceType = ScoreSource.None;
        private float lastSourceValue = 0f;

        private void AddScoreSource(float change, ScoreSource source)
        {
            if (change <= 0 || source == ScoreSource.None) return;

            // Batching
            if (source == lastSourceType && activeSources.Count > 0)
            {
                lastSourceValue += change;
                UpdateSourceItem(activeSources[^1], lastSourceValue, source);
                RefreshSourceFadeTimer();
                return;
            }

            // Prep new item
            lastSourceType = source;
            lastSourceValue = change;

            if (activeSources.Count >= maxVisibleSources)
            {
                sourcePool.Release(activeSources[0]);
                activeSources.RemoveAt(0);
            }

            TMPCanvasSc newItem = sourcePool.Get();
            UpdateSourceItem(newItem, lastSourceValue, source);

            // Layout
            newItem.rectTransform.pivot = new Vector2(0.5f, 1);
            newItem.rectTransform.anchorMin = new Vector2(0.5f, 1);
            newItem.rectTransform.anchorMax = new Vector2(0.5f, 1);
            newItem.rectTransform.anchoredPosition = new Vector2(0, itemHeight);
            activeSources.Add(newItem);

            RefreshSourceFadeTimer();

            if (scrollScoreSourceCoroutine != null) StopCoroutine(scrollScoreSourceCoroutine);
            scrollScoreSourceCoroutine = StartCoroutine(ScrollItemsRoutine());
        }

        private void UpdateSourceItem(TMPCanvasSc item, float value, ScoreSource source)
        {
            string sourceTextString = source switch
            {
                ScoreSource.Roce => $"+{MathF.Round(value, 2)} ROCE",
                ScoreSource.Wheelie => $"+{MathF.Round(value, 2)} CABALLITO",
                ScoreSource.MaxSpeed => $"+{MathF.Round(value, 2)} VELOCIDAD",
                ScoreSource.TripStart => $"+{MathF.Round(value, 2)} PASAJERO",
                ScoreSource.TripEnd => $"+{MathF.Round(value, 2)} VIAJE",
                _ => ""
            };
            item.tmp.text = sourceTextString;
        }

        private void RefreshSourceFadeTimer()
        {
            UtilsSc.SetCanvasAlpha(scoreSourceContainer.canvasGroup, maxAlpha);
            if (fadeScoreSourceCoroutine != null) StopCoroutine(fadeScoreSourceCoroutine);
            fadeScoreSourceCoroutine = StartCoroutine(UtilsSc.FadeRoutine(displayDuration, fadeDuration, maxAlpha, scoreSourceContainer.canvasGroup, () =>
            {
                ClearAllSources();
                fadeScoreSourceCoroutine = null;
            }));
        }

        private void ClearAllSources()
        {
            foreach (var item in activeSources) sourcePool.Release(item);
            activeSources.Clear();
            lastSourceType = ScoreSource.None;
        }

        private IEnumerator ScrollItemsRoutine()
        {
            float elapsed = 0f;
            int count = activeSources.Count;
            float[] startYs = new float[count];
            float[] targetYs = new float[count];

            for (int i = 0; i < count; i++)
            {
                startYs[i] = activeSources[i].rectTransform.anchoredPosition.y;
                // Calculate target Y based on index, with the newest item at the top (0) and older items below it
                // It avoids loosing real Y target when this coroutine is restarted before finishing, allowing to keep the scroll movement fluid even with rapid score source additions
                targetYs[i] = -(count - 1 - i) * itemHeight;
            }

            while (elapsed < scrollDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / scrollDuration;
                float easedT = t * t * (3f - 2f * t);

                for (int i = 0; i < activeSources.Count; i++)
                {
                    activeSources[i].rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(startYs[i], targetYs[i], easedT));
                }
                yield return null;
            }

            for (int i = 0; i < activeSources.Count; i++)
            {
                activeSources[i].rectTransform.anchoredPosition = new Vector2(0, targetYs[i]);
            }

            scrollScoreSourceCoroutine = null;
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
