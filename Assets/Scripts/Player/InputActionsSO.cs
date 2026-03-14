using UnityEngine;
using UnityEngine.InputSystem;

namespace Mototaxi.Player
{
    [CreateAssetMenu(fileName = "InputActions", menuName = "Mototaxi/Player/InputActions", order = 0)]
    public class InputActionsSO : ScriptableObject
    {
        [SerializeField] private InputActionReference _accelerateAction;
        public InputActionReference AccelerateAction => _accelerateAction;
        [SerializeField] private InputActionReference _brakeReverseAction;
        public InputActionReference BrakeReverseAction => _brakeReverseAction;
        [SerializeField] private InputActionReference _steeringAction;
        public InputActionReference SteeringAction => _steeringAction;
        [SerializeField] private InputActionReference _brakeAction;
        public InputActionReference BrakeAction => _brakeAction;
        [SerializeField] private InputActionReference _wheelieAction;
        public InputActionReference WheelieAction => _wheelieAction;
        [SerializeField] private InputActionReference _stoppieAction;
        public InputActionReference StoppieAction => _stoppieAction;
        [SerializeField] private InputActionReference _pauseAction;
        public InputActionReference PauseAction => _pauseAction;
        [SerializeField] private InputActionReference _switchCameraAction;
        public InputActionReference SwitchCameraAction => _switchCameraAction;
    }
}