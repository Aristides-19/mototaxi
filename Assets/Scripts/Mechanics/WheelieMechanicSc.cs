using Mototaxi.Core;
using UnityEngine;
using ArcadeBP_Pro;

namespace Mototaxi.Mechanics
{
    [RequireComponent(typeof(ArcadeBikeControllerPro), typeof(Rigidbody))]
    public class WheelieMechanicSc : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameDataSc GameData;

        private ArcadeBikeControllerPro bikeController;
        private Rigidbody rb;
        private Transform wheelieTransform;

        void Awake()
        {
            bikeController = GetComponent<ArcadeBikeControllerPro>();
            rb = GetComponent<Rigidbody>();
            wheelieTransform = bikeController.bikeReferences.WheelieTransform;

            if (GameData == null) Debug.LogError("GameData reference is missing in WheelieMechanicSc.");
            if (wheelieTransform == null) Debug.LogError("Wheelie Transform reference is missing in ArcadeBikeControllerPro.");
        }

        void Update()
        {
            float currentAngle = Mathf.DeltaAngle(0, wheelieTransform.localEulerAngles.x);

            if (bikeController.isDoingWheelie &&
                rb.linearVelocity.magnitude > GameData.WheelieSettings.MinVelocity &&
                currentAngle <= -GameData.WheelieSettings.MinInclineAngle)
            {
                ScoreManagerSc.AddScore(GameData.WheelieSettings.PointsPerSecond * Time.deltaTime, ScoreSource.Wheelie);
            }
        }
    }
}