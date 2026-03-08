using UnityEngine;
using UnityEngine.UI;

namespace Mototaxi.HUD
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform), typeof(RectMask2D))]
    public class MaskContainerSc : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public RectTransform rectTransform;
        public RectMask2D rectMask2D;

        private void OnValidate()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            rectMask2D = GetComponent<RectMask2D>();
        }
    }
}