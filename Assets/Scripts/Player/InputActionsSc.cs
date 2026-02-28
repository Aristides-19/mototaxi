using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mototaxi.Player
{
    [CreateAssetMenu(fileName = "InputActions", menuName = "Mototaxi/Player/InputActions", order = 0)]
    public class InputActionsSc : ScriptableObject
    {
        public InputActionReference accelerateAction;
        public InputActionReference brakeReverseAction;
        public InputActionReference steeringAction;
        public InputActionReference brakeAction;
        public InputActionReference wheelieAction;
        public InputActionReference stoppieAction;
        public InputActionReference pauseAction;
        public InputActionReference switchCameraAction;
    }
}