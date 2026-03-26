using Mototaxi.Core;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Mototaxi.HUD
{
    public class HintSc : MonoBehaviour
    {
        private Coroutine _currentHintCoroutine;

        #region Inspector Settings
        [Header("Score Display")]
        [SerializeField] TMPCanvasSc hintText;

        [Header("Game Data")]
        [SerializeField] private GameDataSO gameData;

        [Header("Opacity Settings")]
        [SerializeField] float fadeDuration = 0.5f;
        [SerializeField] float displayDuration = 1f;
        [SerializeField] float maxAlpha = 1f;
        #endregion

        private void Awake()
        {
            if (hintText == null) Debug.LogError("Hint TextMeshProUGUI reference is missing in HintSc.");
            UtilsSc.SetCanvasAlpha(hintText.canvasGroup, 0f);
        }

        private void OnEnable()
        {
            HintManagerSc.OnShowHint += ShowHint;
            HintManagerSc.OnHideHint += HideHint;
        }

        private void OnDisable()
        {
            HintManagerSc.OnShowHint -= ShowHint;
            HintManagerSc.OnHideHint -= HideHint;
        }

        private void ShowHint(HintType type, float? duration)
        {
            string hintString = type switch
            {
                HintType.PickUpPassenger => "Presiona 'E' para recoger al pasajero",
                HintType.TripStart => "¡Viaje iniciado! El destino está marcado en la brújula",
                HintType.TripEnd => "¡Viaje completado! Busca otro pasajero",
                HintType.Start => "Recoge pasajeros y llévalos a su destino para ganar dinero",
                HintType.StartTime => GetTimeHintText(),
                _ => ""
            };

            hintText.tmp.text = hintString;

            if (_currentHintCoroutine != null) StopCoroutine(_currentHintCoroutine);

            UtilsSc.SetCanvasAlpha(hintText.canvasGroup, maxAlpha);

            if (duration > 0 || duration == null)
            {
                _currentHintCoroutine = StartCoroutine(UtilsSc.FadeRoutine(duration ?? displayDuration, fadeDuration, maxAlpha, hintText.canvasGroup, null));
            }
        }

        private void HideHint()
        {
            if (_currentHintCoroutine != null) StopCoroutine(_currentHintCoroutine);
            _currentHintCoroutine = StartCoroutine(UtilsSc.FadeRoutine(0f, fadeDuration, hintText.canvasGroup.alpha, hintText.canvasGroup, null));
        }

        private string GetTimeHintText()
        {
            int minutes = Mathf.FloorToInt(gameData.TripSettings.MaxGameDuration / 60);
            return $"¡Completa tantos viajes como puedas en {minutes} minutos!";
        }
    }

    public static class HintManagerSc
    {
        public static event System.Action<HintType, float?> OnShowHint;
        public static event System.Action OnHideHint;

        /// <summary>
        /// Shows a hint. Pass duration 0 or negative to keep it visible until HideHint is called.
        /// </summary>
        public static void TriggerHint(HintType type, float? duration = null)
        {
            OnShowHint?.Invoke(type, duration);
        }

        public static void HideHint()
        {
            OnHideHint?.Invoke();
        }
    }

    public enum HintType
    {
        None,
        PickUpPassenger,
        TripStart,
        TripEnd,
        Start,
        StartTime
    }
}