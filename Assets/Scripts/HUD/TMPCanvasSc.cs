using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    [RequireComponent(typeof(TextMeshProUGUI), typeof(CanvasGroup))]
    public class TMPCanvasSc : MonoBehaviour
    {
        public TextMeshProUGUI tmp;
        public CanvasGroup canvasGroup;

        private void OnValidate()
        {
            tmp = GetComponent<TextMeshProUGUI>();
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}