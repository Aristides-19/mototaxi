using Mototaxi.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ArcadeBP_Pro
{
    [RequireComponent(typeof(ArcadeBikeControllerPro))]
    public class BikeInputProvider : MonoBehaviour
    {
        [Header("Player Actions")]
        [SerializeField] InputActionsSc input;

        private float Accelerate, Reverse, HandBrake, SteeringLeft, SteeringRight, Wheelie, Stoppie;
        private ArcadeBikeControllerPro arcadeBikeControllerPro;

        void Awake()
        {
            arcadeBikeControllerPro = GetComponent<ArcadeBikeControllerPro>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            SetPlayerInput();
        }

        private void SetPlayerInput()
        {
            float steering = input.steeringAction.action.ReadValue<float>();

            Accelerate = input.accelerateAction.action.IsPressed() ? 1f : 0f;
            Reverse = input.brakeReverseAction.action.IsPressed() ? 1f : 0f;
            SteeringLeft = (steering < 0) ? 1f : 0f;
            SteeringRight = (steering > 0) ? 1f : 0f;

            HandBrake = input.brakeAction.action.IsPressed() ? 1f : 0f;
            Wheelie = input.wheelieAction.action.IsPressed() ? 1f : 0f;
            Stoppie = input.stoppieAction.action.IsPressed() ? 1f : 0f;

            // Note : You can also use your custom inputs above to provide inputs to the bike controller
            // provide inputs to the bike controller
            arcadeBikeControllerPro.provideInput(Accelerate, Reverse, HandBrake, SteeringLeft, SteeringRight, Wheelie, Stoppie);
        }

    }
}
