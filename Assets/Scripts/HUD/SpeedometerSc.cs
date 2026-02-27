using ArcadeBP_Pro;
using TMPro;
using UnityEngine;

namespace Mototaxi.HUD
{
    public class SpeedometerSc : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI speedText;
        [SerializeField] TextMeshProUGUI gearText;
        [SerializeField] ArcadeBikeControllerPro bikeController;

        void Awake()
        {
            if (bikeController == null)
            {
                Debug.LogError("ArcadeBikeControllerPro reference is missing in SpeedometerSc.");
            }
            if (speedText == null)
            {
                Debug.LogError("Speed TextMeshProUGUI reference is missing in SpeedometerSc.");
            }
            if (gearText == null)
            {
                Debug.LogError("Gear TextMeshProUGUI reference is missing in SpeedometerSc.");
            }
        }

        void Update()
        {
            float speed = bikeController.localBikeVelocity.magnitude * 3.6f;
            int gear = bikeController.currentGear;
            speedText.text = Mathf.RoundToInt(speed).ToString() + " km/h";
            gearText.text = "Cambio: " + gear.ToString();
        }
    }
}
