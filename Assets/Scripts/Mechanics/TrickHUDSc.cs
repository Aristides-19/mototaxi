using UnityEngine;
using TMPro;
using Mototaxi.Core;

namespace Mototaxi.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TrickHUDSc : MonoBehaviour
    {
        private TextMeshProUGUI trickText;

        void Awake()
        {
            trickText = GetComponent<TextMeshProUGUI>();
            trickText.text = "";
        }

        void OnEnable() => ScoreManagerSc.OnTrickUpdated += UpdateText;

        void OnDisable() => ScoreManagerSc.OnTrickUpdated -= UpdateText;

        private void UpdateText(string trickName, int points)
        {
            if (points <= 0)
            {
                trickText.text = "";
            }
            else
            {
                trickText.text = $"{trickName} +{points}";
            }
        }
    }
}