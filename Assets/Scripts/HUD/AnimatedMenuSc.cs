using UnityEngine;
using System.Collections;

namespace Mototaxi.HUD
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AnimatedMenuSc : MonoBehaviour
    {
        [SerializeField] float fadeDuration = 0.2f;
        [SerializeField] bool startActive = false;
        private CanvasGroup _canvasGroup;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            SetState(startActive, true);
        }

        public void SetState(bool active, bool immediate = false)
        {
            StopAllCoroutines();
            float targetAlpha = active ? 1 : 0;

            if (immediate)
            {
                _canvasGroup.alpha = targetAlpha;
                UpdateInteraction(active);
            }
            else
            {
                StartCoroutine(Fade(targetAlpha, active));
            }
        }

        IEnumerator Fade(float target, bool active)
        {
            // Should disable interaction before fading out to prevent clicks during fade-out
            if (!active) UpdateInteraction(false);

            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / fadeDuration);
                yield return null;
            }

            if (active) UpdateInteraction(true);
        }

        private void UpdateInteraction(bool active)
        {
            _canvasGroup.interactable = active;
            _canvasGroup.blocksRaycasts = active;
        }
    }
}