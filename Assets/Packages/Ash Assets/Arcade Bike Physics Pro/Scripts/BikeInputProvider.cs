using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ArcadeBP_Pro
{
    public class BikeInputProvider : MonoBehaviour
    {
        public ArcadeBikeControllerPro arcadeBikeControllerPro;

        [Header("Player Actions")]
        [SerializeField] InputActionReference moveAction;
        [SerializeField] InputActionReference brakeAction;
        [SerializeField] InputActionReference wheelieAction;

        private float Accelerate, Reverse, HandBrake, SteeringLeft, SteeringRight, Wheelie;

        private void Update()
        {
            SetPlayerInput();
        }

        private void SetPlayerInput()
        {
            UnityEngine.Vector2 moveInput = moveAction.action.ReadValue<UnityEngine.Vector2>();

            Accelerate = (moveInput.y > 0) ? 1f : 0f;
            Reverse = (moveInput.y < 0) ? 1f : 0f;
            SteeringLeft = (moveInput.x < 0) ? 1f : 0f;
            SteeringRight = (moveInput.x > 0) ? 1f : 0f;

            HandBrake = brakeAction.action.IsPressed() ? 1f : 0f;
            Wheelie = wheelieAction.action.IsPressed() ? 1f : 0f;

            // Note : You can also use your custom inputs above to provide inputs to the bike controller
            // provide inputs to the bike controller
            arcadeBikeControllerPro.provideInput(Accelerate, Reverse, HandBrake, SteeringLeft, SteeringRight, Wheelie);
        }

    }
}
