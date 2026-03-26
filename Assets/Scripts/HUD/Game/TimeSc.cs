using System;
using Mototaxi.Core;
using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    public class TimeSc : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI timeText;

        private void Awake()
        {
            if (timeText == null) Debug.LogError("Time TextMeshProUGUI reference is missing in TimeSc.");
            UpdateTimeDisplay(TimeManagerSc.ElapsedTime);
        }

        private void OnEnable() => TimeManagerSc.OnSecondPassed += UpdateTimeDisplay;
        private void OnDisable() => TimeManagerSc.OnSecondPassed -= UpdateTimeDisplay;

        private void UpdateTimeDisplay(float elapsedTime)
        {
            TimeSpan t = TimeSpan.FromSeconds(elapsedTime);
            timeText.text = $"<size=+20>{t.Minutes:D2}</size>:<size=+10>{t.Seconds:D2}</size>";
        }
    }
}