using ArcadeBP_Pro;
using UnityEngine;

namespace Mototaxi.Passenger
{
    public class PassengerAnimationControllerSc : MonoBehaviour
    {
        [Tooltip("Reference to the bike controller script.")]
        [SerializeField] ArcadeBikeControllerPro bikeController;

        [Tooltip("Reference to the passenger animation targets.")]
        [SerializeField] PassengerAnimationTargetsSc passengerTargets;

        [Header("Leaning Parameters")]
        [Tooltip("Maximum sideways offset for the passenger's hip.")]
        [SerializeField] float maxHipPosOffset = 0.15f;

        [Tooltip("Maximum rotation for the passenger's hip.")]
        [SerializeField] float maxHipRotOffset = 15.0f;

        [Tooltip("Maximum rotation for the passenger's spine.")]
        [SerializeField] float maxSpineRotOffset = 10.0f;

        [Tooltip("Intensity multiplier for the leaning movement.")]
        [Range(0, 2)]
        [SerializeField] float leanIntensity = 1.0f;

        [Tooltip("Speed of the leaning animation transition.")]
        [SerializeField] float leanSpeed = 15.0f;

        [Tooltip("Speed of transitioning between different animation states.")]
        [SerializeField] float transitionSpeed = 5.0f;

        [Header("Rig References")]
        [SerializeField] Transform hipTarget_rig;
        [SerializeField] Transform spineRootTarget_rig;
        [SerializeField] Transform spineTipTarget_rig;
        [SerializeField] Transform rightLegTarget_rig;
        [SerializeField] Transform leftLegTarget_rig;
        [SerializeField] Transform rightHandTarget_rig;
        [SerializeField] Transform leftHandTarget_rig;

        private Vector3 leanPosOffsetForHip;
        private Vector3 leanRotOffsetForHip;
        private Vector3 leanRotOffsetForSpine;
        private float leanLerp = 0.0f;

        void Start()
        {
            InitializeRig();
        }

        void InitializeRig()
        {
            hipTarget_rig.localPosition = passengerTargets.hipTarget.localPosition;
            hipTarget_rig.localRotation = passengerTargets.hipTarget.localRotation;

            spineRootTarget_rig.localRotation = passengerTargets.spineTarget.localRotation;
            spineTipTarget_rig.localRotation = passengerTargets.spineTarget.localRotation;

            rightLegTarget_rig.localPosition = passengerTargets.rightLegTarget.localPosition;
            leftLegTarget_rig.localPosition = passengerTargets.leftLegTarget.localPosition;

            rightHandTarget_rig.position = passengerTargets.rightHandTarget.position;
            leftHandTarget_rig.position = passengerTargets.leftHandTarget.position;
        }

        void Update()
        {
            HandleLeaning();
            UpdateRigPositions();
        }

        void HandleLeaning()
        {
            // Lean smooth transition
            if (Mathf.Abs(bikeController.CurrentSteerInput) > 0)
            {
                leanLerp = Mathf.Lerp(leanLerp, 1, Time.deltaTime * leanSpeed);
            }
            else
            {
                leanLerp = Mathf.Lerp(leanLerp, 0, Time.deltaTime * leanSpeed);
            }

            float leanAngle = bikeController.currentLeanAngle * leanIntensity;
            float maxLean = bikeController.bikeSettings.maxLeanAngle;

            // Hip Offset Calculation
            float posX = -leanAngle / maxLean * maxHipPosOffset;
            leanPosOffsetForHip = new Vector3(posX, -Mathf.Abs(posX / 2), 0);

            // Hip Rotation Calculation
            float RotZ_Hip = leanAngle / maxLean * maxHipRotOffset * leanLerp;
            leanRotOffsetForHip = new Vector3(0, 0, RotZ_Hip);

            // Spine Rotation Calculation
            float RotZ_spine = leanAngle / maxLean * maxSpineRotOffset;
            leanRotOffsetForSpine = new Vector3(0, 0, RotZ_spine);
        }

        void UpdateRigPositions()
        {
            // Update Hip
            TransitionToTarget(hipTarget_rig, passengerTargets.hipTarget, leanPosOffsetForHip, leanRotOffsetForHip);

            // Update Spine
            TransitionToTarget(spineRootTarget_rig, passengerTargets.spineTarget, Vector3.zero, leanRotOffsetForSpine);
            TransitionToTarget(spineTipTarget_rig, passengerTargets.spineTarget, Vector3.zero, leanRotOffsetForSpine);

            // Update static hands and legs
            rightHandTarget_rig.position = Vector3.Lerp(rightHandTarget_rig.position, passengerTargets.rightHandTarget.position, Time.deltaTime * transitionSpeed);
            rightHandTarget_rig.rotation = Quaternion.Slerp(rightHandTarget_rig.rotation, passengerTargets.rightHandTarget.rotation, Time.deltaTime * transitionSpeed);

            leftHandTarget_rig.position = Vector3.Lerp(leftHandTarget_rig.position, passengerTargets.leftHandTarget.position, Time.deltaTime * transitionSpeed);
            leftHandTarget_rig.rotation = Quaternion.Slerp(leftHandTarget_rig.rotation, passengerTargets.leftHandTarget.rotation, Time.deltaTime * transitionSpeed);

            rightLegTarget_rig.position = Vector3.Lerp(rightLegTarget_rig.position, passengerTargets.rightLegTarget.position, Time.deltaTime * transitionSpeed);
            rightLegTarget_rig.rotation = Quaternion.Slerp(rightLegTarget_rig.rotation, passengerTargets.rightLegTarget.rotation, Time.deltaTime * transitionSpeed);

            leftLegTarget_rig.position = Vector3.Lerp(leftLegTarget_rig.position, passengerTargets.leftLegTarget.position, Time.deltaTime * transitionSpeed);
            leftLegTarget_rig.rotation = Quaternion.Slerp(leftLegTarget_rig.rotation, passengerTargets.leftLegTarget.rotation, Time.deltaTime * transitionSpeed);
        }

        void TransitionToTarget(Transform target, Transform desiredTransform, Vector3 PositionOffset, Vector3 RotationOffset)
        {
            Vector3 targetPosition = desiredTransform.TransformPoint(PositionOffset);
            target.position = Vector3.Lerp(target.position, targetPosition, Time.deltaTime * transitionSpeed);

            Quaternion desiredRotationWithOffset = desiredTransform.rotation * Quaternion.Euler(RotationOffset);
            target.rotation = Quaternion.Slerp(target.rotation, desiredRotationWithOffset, Time.deltaTime * transitionSpeed);
        }
    }
}
