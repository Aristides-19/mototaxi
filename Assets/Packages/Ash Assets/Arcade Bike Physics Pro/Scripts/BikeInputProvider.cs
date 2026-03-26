using Gley.TrafficSystem.Internal;
using Mototaxi.Core;
using Mototaxi.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ArcadeBP_Pro
{
    [RequireComponent(typeof(ArcadeBikeControllerPro))]
    public class BikeInputProvider : MonoBehaviour
    {
        [Header("Player Actions")]
        [SerializeField] InputActionsSO input;
        [SerializeField] private bool _inControl = true;

        private float Accelerate, Reverse, HandBrake, SteeringLeft, SteeringRight, Wheelie, Stoppie;
        private ArcadeBikeControllerPro arcadeBikeControllerPro;

        void Awake()
        {
            arcadeBikeControllerPro = GetComponent<ArcadeBikeControllerPro>();
            if (_inControl)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OnEnable()
        {
            TimeManagerSc.OnTimeUp += DisableControl;
        }

        private void OnDisable()
        {
            TimeManagerSc.OnTimeUp -= DisableControl;
        }

        private void DisableControl() => _inControl = false;

        private void Update()
        {
            if (_inControl) SetPlayerInput();
            else arcadeBikeControllerPro.provideInput(0f, 0f, 1f, 0f, 0f, 0f, 0f);
        }

        private void SetPlayerInput()
        {
            float steering = input.SteeringAction.action.ReadValue<float>();

            Accelerate = input.AccelerateAction.action.IsPressed() ? 1f : 0f;
            Reverse = input.BrakeReverseAction.action.IsPressed() ? 1f : 0f;
            SteeringLeft = (steering < 0) ? 1f : 0f;
            SteeringRight = (steering > 0) ? 1f : 0f;

            HandBrake = input.BrakeAction.action.IsPressed() ? 1f : 0f;
            Wheelie = input.WheelieAction.action.IsPressed() ? 1f : 0f;
            Stoppie = input.StoppieAction.action.IsPressed() ? 1f : 0f;

            // Note : You can also use your custom inputs above to provide inputs to the bike controller
            // provide inputs to the bike controller
            arcadeBikeControllerPro.provideInput(Accelerate, Reverse, HandBrake, SteeringLeft, SteeringRight, Wheelie, Stoppie);
        }

    }
}
