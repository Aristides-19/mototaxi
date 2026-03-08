using Mototaxi.Core;
using UnityEngine;
using ArcadeBP_Pro;

namespace Mototaxi.Mechanics
{
    [RequireComponent(typeof(ArcadeBikeControllerPro))]
    public class WheelieMechanicSc : MonoBehaviour
    {
        [SerializeField] Rigidbody playerRigidbody;
        [SerializeField] GameDataSc GameData;
        [SerializeField] Transform modeloVisualMoto;

        private ArcadeBikeControllerPro bikeController;
        private float currentWheeliePoints = 0f;
        private bool isTrickActive = false;

        void Awake()
        {
            bikeController = GetComponent<ArcadeBikeControllerPro>();

            if (playerRigidbody == null) Debug.LogError("Falta Rigidbody.");
            if (GameData == null) Debug.LogError("Falta asignar el GameData.");
        }

        void Update()
        {
            if (GameData == null || bikeController == null || modeloVisualMoto == null) return;

            float speed = playerRigidbody.linearVelocity.magnitude;
            bool estaHaciendoWheelie = bikeController.isDoingWheelie;

            float currentIncline = Mathf.Asin(modeloVisualMoto.forward.y) * Mathf.Rad2Deg;

            if (estaHaciendoWheelie && speed > GameData.WheelieSettings.MinVelocity && currentIncline >= GameData.WheelieSettings.MinInclineAngle)
            {
                float pointsEarned = GameData.WheelieSettings.PointsPerSecond * Time.deltaTime;

                ScoreManagerSc.AddScore(pointsEarned);
                currentWheeliePoints += pointsEarned;
                isTrickActive = true;

                ScoreManagerSc.UpdateTrickUI("¡CABALLITO!", Mathf.FloorToInt(currentWheeliePoints));
            }
            else
            {
                if (isTrickActive)
                {
                    ScoreManagerSc.UpdateTrickUI("", 0);
                    currentWheeliePoints = 0f;
                    isTrickActive = false;
                }
            }
        }
    }
}