using Mototaxi.Core;
using Mototaxi.Mechanics;
using UnityEngine;
using TMPro;
using ArcadeBP_Pro;

namespace Mototaxi.Mechanics
{
    [RequireComponent(typeof(ArcadeBikeControllerPro))]
    public class WheelieMechanicSc : MonoBehaviour
    {
        [SerializeField] Rigidbody playerRigidbody;
        [SerializeField] GameDataSc GameData;

        [Header("UI Visuals")]
        [SerializeField] TextMeshProUGUI wheelieText;
        [SerializeField] Transform modeloVisualMoto; // Volvemos a pedir el modelo 3D

        private ArcadeBikeControllerPro bikeController;
        private float currentWheeliePoints = 0f;

        void Awake()
        {
            bikeController = GetComponent<ArcadeBikeControllerPro>();

            if (playerRigidbody == null) Debug.LogError("Falta Rigidbody.");
            if (GameData == null) Debug.LogError("Falta asignar el GameData.");
            if (wheelieText != null) wheelieText.gameObject.SetActive(false);
        }

        void Update()
        {
            if (GameData == null || bikeController == null || modeloVisualMoto == null) return;

            float speed = playerRigidbody.linearVelocity.magnitude;
            bool estaHaciendoWheelie = bikeController.isDoingWheelie;

            // Calculamos el ángulo real de la moto
            float currentIncline = Mathf.Asin(modeloVisualMoto.forward.y) * Mathf.Rad2Deg;

            // AHORA EXIGIMOS LAS 3 COSAS: El wheelie de Arístides + Velocidad + Ángulo de altura
            if (estaHaciendoWheelie && speed > GameData.WheelieSettings.MinVelocity && currentIncline >= GameData.WheelieSettings.MinInclineAngle)
            {
                float pointsEarned = GameData.WheelieSettings.PointsPerSecond * Time.deltaTime;

                ScoreManagerSc.AddScore(pointsEarned);
                currentWheeliePoints += pointsEarned;

                if (wheelieText != null)
                {
                    wheelieText.gameObject.SetActive(true);
                    wheelieText.text = "¡CABALLITO! +" + Mathf.FloorToInt(currentWheeliePoints).ToString();
                }
            }
            else
            {
                if (wheelieText != null && wheelieText.gameObject.activeSelf)
                {
                    wheelieText.gameObject.SetActive(false);
                    currentWheeliePoints = 0f;
                }
            }
        }
    }
}