using ArcadeBP_Pro;
using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    public class SpeedometerSc : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI speedText;
        [SerializeField] TextMeshProUGUI gearText;

        private void Awake()
        {
            if (speedText == null)
            {
                Debug.LogError("Speed TextMeshProUGUI reference is missing in SpeedometerSc.");
            }
            if (gearText == null)
            {
                Debug.LogError("Gear TextMeshProUGUI reference is missing in SpeedometerSc.");
            }
        }

        private void OnEnable()
        {
            ArcadeBikeControllerPro.OnCurrentGearChange += UpdateGearText;
            ArcadeBikeControllerPro.OnLocalVelocityChange += UpdateSpeedText;
        }

        private void OnDisable()
        {
            ArcadeBikeControllerPro.OnCurrentGearChange -= UpdateGearText;
            ArcadeBikeControllerPro.OnLocalVelocityChange -= UpdateSpeedText;
        }

        private void UpdateGearText(int gear)
        {
            gearText.text = "Cambio: " + gear.ToString();
        }

        private void UpdateSpeedText(Vector3 speed)
        {
            speedText.text = Mathf.RoundToInt(speed.magnitude * 3.6f).ToString() + " km/h";
        }
    }
}
