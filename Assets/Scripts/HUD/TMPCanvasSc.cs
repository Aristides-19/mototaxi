using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    [RequireComponent(typeof(TextMeshProUGUI), typeof(CanvasGroup), typeof(RectTransform))]
    public class TMPCanvasSc : MonoBehaviour
    {
        public TextMeshProUGUI tmp;
        public CanvasGroup canvasGroup;
        public RectTransform rectTransform;

        private void OnValidate()
        {
            tmp = GetComponent<TextMeshProUGUI>();
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
        }
    }
}