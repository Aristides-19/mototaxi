using UnityEngine;

namespace Mototaxi.Passenger
{
    public class BikePassengerAnimTargetsSc : MonoBehaviour
    {
        [Header("Hip Target")]
        public Transform hipTarget;

        [Header("Spine Target")]
        public Transform spineTarget;

        [Header("Hand Targets")]
        public Transform leftHandTarget;
        public Transform rightHandTarget;

        [Header("Leg Targets")]
        public Transform leftLegTarget;
        public Transform rightLegTarget;
    }
}
