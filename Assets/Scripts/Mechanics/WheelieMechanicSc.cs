using Mototaxi.Core;
using UnityEngine;
using ArcadeBP_Pro;
using Mototaxi.Player;

namespace Mototaxi.Mechanics
{
    [RequireComponent(typeof(ArcadeBikeControllerPro), typeof(Rigidbody))]
    public class WheelieMechanicSc : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameDataSO GameData;

        private ArcadeBikeControllerPro bikeController;
        private Rigidbody rb;
        private Transform wheelieTransform;
        private float wheelieTimer;

        void Awake()
        {
            bikeController = GetComponent<ArcadeBikeControllerPro>();
            rb = GetComponent<Rigidbody>();
            wheelieTransform = bikeController.bikeReferences.WheelieTransform;

            if (GameData == null) Debug.LogError("GameData reference is missing in WheelieMechanicSc.");
        }

        void OnEnable() => ArcadeBikeControllerPro.OnWheelieStateChange += OnWheelieStateChange;
        void OnDisable() => ArcadeBikeControllerPro.OnWheelieStateChange -= OnWheelieStateChange;

        void OnWheelieStateChange(bool isDoingWheelie)
        {
            if (!PlayerStateSc.IsOnTrip) return;

            float currentAngle = Mathf.DeltaAngle(0, wheelieTransform.localEulerAngles.x);

            if (isDoingWheelie &&
                rb.linearVelocity.magnitude > GameData.WheelieSettings.MinVelocity &&
                currentAngle <= -GameData.WheelieSettings.MinInclineAngle)
            {
                wheelieTimer += Time.deltaTime;
                if (wheelieTimer >= GameData.WheelieSettings.IntervalToStartScoring)
                {
                    ScoreManagerSc.AddScore(GameData.WheelieSettings.PointsPerSecond * Time.deltaTime, ScoreSource.Wheelie);
                    wheelieTimer = 0f;
                }
            }
            else
            {
                wheelieTimer = 0f;
            }
        }
    }
}