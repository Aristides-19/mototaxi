using System.Collections;
using UnityEngine;

namespace Mototaxi.HUD
{
    static class UtilsSc
    {
        public static void SetCanvasAlpha(CanvasGroup canvas, float alpha)
        {
            canvas.alpha = alpha;
        }

        public static IEnumerator FadeRoutine(float displayDuration, float fadeDuration, float maxAlpha, CanvasGroup canvas, System.Action onComplete)
        {
            yield return new WaitForSeconds(displayDuration);

            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float newAlpha = Mathf.Lerp(maxAlpha, 0f, elapsed / fadeDuration);

                SetCanvasAlpha(canvas, newAlpha);
                yield return null;
            }

            SetCanvasAlpha(canvas, 0f);
            onComplete?.Invoke();
        }

        public static IEnumerator PunchScaleRoutine(Vector3 originalScale, float punchScale, float punchDuration, Transform transform, System.Action onComplete)
        {
            float elapsed = 0f;
            Vector3 targetScale = originalScale * punchScale;

            while (elapsed < punchDuration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / punchDuration);
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < punchDuration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / punchDuration);
                yield return null;
            }

            transform.localScale = originalScale;
            onComplete?.Invoke();
        }
    }
}