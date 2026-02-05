using UnityEngine;
using UnityEngine.InputSystem;

namespace ArcadeBP_Pro
{
    public class BikeInputProvider : MonoBehaviour
    {
        public ArcadeBikeControllerPro arcadeBikeControllerPro;

        [Header("Player Actions")]
        [SerializeField] InputActionReference accelerateAction;
        [SerializeField] InputActionReference brakeReverseAction;
        [SerializeField] InputActionReference steeringAction;
        [SerializeField] InputActionReference brakeAction;
        [SerializeField] InputActionReference wheelieAction;

        private float Accelerate, Reverse, HandBrake, SteeringLeft, SteeringRight, Wheelie;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            SetPlayerInput();
        }

        private void SetPlayerInput()
        {
            float steering = steeringAction.action.ReadValue<float>();

            Accelerate = accelerateAction.action.IsPressed() ? 1f : 0f;
            Reverse = brakeReverseAction.action.IsPressed() ? 1f : 0f;
            SteeringLeft = (steering < 0) ? 1f : 0f;
            SteeringRight = (steering > 0) ? 1f : 0f;

            HandBrake = brakeAction.action.IsPressed() ? 1f : 0f;
            Wheelie = wheelieAction.action.IsPressed() ? 1f : 0f;

            // Note : You can also use your custom inputs above to provide inputs to the bike controller
            // provide inputs to the bike controller
            arcadeBikeControllerPro.provideInput(Accelerate, Reverse, HandBrake, SteeringLeft, SteeringRight, Wheelie);
        }

    }
}
