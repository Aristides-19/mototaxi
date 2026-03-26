using System;
using Mototaxi.Passenger;
using UnityEngine;


namespace ArcadeBP_Pro
{
    [RequireComponent(typeof(ArcadeBikeControllerPro), typeof(Rigidbody))]
    public class RagdollActivator : MonoBehaviour
    {
        [Tooltip("Reference to the CameraController script.")]
        public CameraController cameraController;

        [Tooltip("Prefab for the Dummy bike.")]
        public GameObject dummyBikePrefab;

        [Tooltip("Prefab for the character ragdoll.")]
        public GameObject characterRagdollPrefab;

        [Tooltip("Animator component of the animated character.")]
        public Animator characterAnimator;

        [Header("Passenger Settings")]
        [Tooltip("Prefab for the passenger ragdoll (optional).")]
        public BikePassengerSc passengerRagdollPrefab;

        [Tooltip("Animator component of the animated passenger (optional).")]
        public Animator passengerAnimator;

        [Tooltip("Threshold of impact force to activate ragdoll.")]
        public float impactThreshold = 10f;

        [Tooltip("Ignore collisions with the bottom part of the bike collider.")]
        public bool IgnoreBottomCollision = true;

        // C# Events
        public event Action OnRagdollActivated;
        public event Action OnBikeReEnabled;


        private Rigidbody bikeRigidbody;
        private bool isRagdollActivated = false;
        private GameObject bikeRagdollInstance;
        public GameObject characterRagdollInstance { get; private set; }
        public GameObject passengerRagdollInstance { get; private set; }
        private Transform hipTransform;
        private Collider bikeCollider;
        public ArcadeBikeControllerPro bikeController;

        void Start()
        {
            bikeController = GetComponent<ArcadeBikeControllerPro>();
            bikeRigidbody = GetComponent<Rigidbody>();
            bikeCollider = bikeController.bikeReferences.collider;

            OnRagdollActivated += setCameraTargetToRagdoll;
            OnBikeReEnabled += resetCameratoBike;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (isRagdollActivated) return;

            Vector3 localContactPoint = bikeController.bikeReferences.LeanTransform.InverseTransformPoint(collision.contacts[0].point);
            Vector3 bikeCenter = bikeController.bikeReferences.LeanTransform.InverseTransformPoint(bikeCollider.bounds.center);

            if (IgnoreBottomCollision)
            {
                if (localContactPoint.y > bikeCenter.y)
                {
                    // Check if the impact is strong enough
                    if (collision.impulse.magnitude / bikeRigidbody.mass > impactThreshold)
                    {
                        ActivateRagdoll();
                    }
                }
            }
            else
            {
                // Check if the impact is strong enough
                if (collision.impulse.magnitude / bikeRigidbody.mass > impactThreshold)
                {
                    ActivateRagdoll();
                }
            }
        }

        void ActivateRagdoll()
        {
            isRagdollActivated = true;

            Transform bikeTransform = bikeController.bikeReferences.LeanTransform;

            // Instantiate the ragdolls
            bikeRagdollInstance = Instantiate(dummyBikePrefab, bikeTransform.position, bikeTransform.rotation);
            characterRagdollInstance = Instantiate(characterRagdollPrefab, bikeTransform.position, bikeTransform.rotation);

            if (passengerAnimator.gameObject.activeInHierarchy)
            {
                passengerRagdollInstance = Instantiate(passengerRagdollPrefab.gameObject, bikeTransform.position, bikeTransform.rotation);
                passengerRagdollInstance.GetComponent<BikePassengerSc>().SetPassenger(passengerAnimator.GetComponent<BikePassengerSc>().CurrentData);

                Animator passengerRagdollAnimator = passengerRagdollInstance.GetComponent<Animator>();
                foreach (HumanBodyBones bone in (HumanBodyBones[])System.Enum.GetValues(typeof(HumanBodyBones)))
                {
                    if (bone == HumanBodyBones.LastBone) continue;

                    Transform characterBoneTransform = passengerAnimator.GetBoneTransform(bone);
                    Transform ragdollBoneTransform = passengerRagdollAnimator.GetBoneTransform(bone);

                    if (characterBoneTransform != null && ragdollBoneTransform != null)
                    {
                        ragdollBoneTransform.rotation = characterBoneTransform.rotation;
                    }
                }
            }

            // Match the ragdoll bones' rotations to the character's bones
            Animator ragdollAnimator = characterRagdollInstance.GetComponent<Animator>();
            foreach (HumanBodyBones bone in (HumanBodyBones[])System.Enum.GetValues(typeof(HumanBodyBones)))
            {
                if (bone == HumanBodyBones.LastBone) continue;

                Transform characterBoneTransform = characterAnimator.GetBoneTransform(bone);
                Transform ragdollBoneTransform = ragdollAnimator.GetBoneTransform(bone);

                if (characterBoneTransform != null && ragdollBoneTransform != null)
                {
                    ragdollBoneTransform.rotation = characterBoneTransform.rotation;
                }
            }

            // Match velocities and forces
            Rigidbody[] bikeRagdollRigidbodies = bikeRagdollInstance.GetComponentsInChildren<Rigidbody>();
            Rigidbody[] characterRagdollRigidbodies = characterRagdollInstance.GetComponentsInChildren<Rigidbody>();

            Vector3 bikeVelocity = bikeRigidbody.linearVelocity;
            Vector3 bikeAngularVelocity = bikeRigidbody.angularVelocity;

            foreach (Rigidbody rb in bikeRagdollRigidbodies)
            {
                rb.linearVelocity = bikeVelocity;
                rb.angularVelocity = bikeAngularVelocity;
            }

            foreach (Rigidbody rb in characterRagdollRigidbodies)
            {
                rb.linearVelocity = bikeVelocity;
                rb.angularVelocity = bikeAngularVelocity;
            }

            if (passengerRagdollInstance != null)
            {

                Rigidbody[] passengerRigidbodies = passengerRagdollInstance.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in passengerRigidbodies)
                {
                    rb.linearVelocity = bikeVelocity;
                    rb.angularVelocity = bikeAngularVelocity;
                }
            }


            // Deactivate the original bike
            gameObject.SetActive(false);
            OnRagdollActivated?.Invoke();
        }

        public void ReEnableBike()
        {
            if (!isRagdollActivated) return;

            // Destroy the ragdoll instances
            Destroy(bikeRagdollInstance);
            Destroy(characterRagdollInstance);
            Destroy(passengerRagdollInstance);

            // Re-enable the original bike
            gameObject.SetActive(true);
            bikeController.canAccelerate = true;
            bikeController.bikeAudio.engineSound.pitch = bikeController.bikeAudio.minPitch;
            bikeController.bikeReferences.LeanTransform.localRotation = Quaternion.identity;
            bikeRigidbody.linearVelocity = Vector3.zero;
            bikeRigidbody.angularVelocity = Vector3.zero;

            isRagdollActivated = false;

            // Invoke the event
            OnBikeReEnabled?.Invoke();
        }

        public void setCameraTargetToRagdoll()
        {
            hipTransform = characterRagdollInstance.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips);
            cameraController.SetCameratarget(hipTransform, hipTransform);
        }

        public void resetCameratoBike()
        {
            cameraController.ResetCameraTarget();
        }

        public void ForceActivateRagdoll()
        {
            if (!isRagdollActivated)
            {
                ActivateRagdoll();
            }
        }

        public void ResetBike()
        {
            if (isRagdollActivated)
            {
                ReEnableBike();
            }
        }
    }

}
