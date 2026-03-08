using System.Collections;
using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    static class TMPUtilsSc
    {
        public static void SetTextAlpha(TextMeshProUGUI text, float alpha)
        {
            Color color = text.color;
            text.color = new Color(color.r, color.g, color.b, alpha);
        }

        public static IEnumerator FadeRoutine(float displayDuration, float fadeDuration, float maxAlpha, TextMeshProUGUI textTMP, System.Action onFadeComplete)
        {
            yield return new WaitForSeconds(displayDuration);

            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float newAlpha = Mathf.Lerp(maxAlpha, 0f, elapsed / fadeDuration);

                SetTextAlpha(textTMP, newAlpha);
                yield return null;
            }

            onFadeComplete?.Invoke();
        }

        public static IEnumerator PunchScaleRoutine(Vector3 originalScale, float punchScale, float punchDuration, TextMeshProUGUI textTMP, System.Action onPunchComplete)
        {
            float elapsed = 0f;
            Vector3 targetScale = originalScale * punchScale;

            while (elapsed < punchDuration)
            {
                elapsed += Time.deltaTime;
                textTMP.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / punchDuration);
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < punchDuration)
            {
                elapsed += Time.deltaTime;
                textTMP.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / punchDuration);
                yield return null;
            }

            textTMP.transform.localScale = originalScale;
            onPunchComplete?.Invoke();
        }
    }
}