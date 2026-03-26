using UnityEngine;
using ArcadeBP_Pro;
using System.Collections.Generic;

namespace Mototaxi.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class CrashSc : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Reference to the RagdollActivator component. If null, it will try to find one.")]
        [SerializeField] private RagdollActivator ragdollActivator;

        [Header("Audio Settings")]
        [Tooltip("List of crash sound effects to play randomly.")]
        [SerializeField] private List<AudioClip> crashSounds;

        [Tooltip("Minimum pitch to apply for variation.")]
        [SerializeField, Range(0.5f, 1.5f)] private float minPitch = 0.85f;

        [Tooltip("Maximum pitch to apply for variation.")]
        [SerializeField, Range(0.5f, 1.5f)] private float maxPitch = 1.15f;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            ragdollActivator.OnRagdollActivated += PlayCrashSound;
        }

        private void OnDisable()
        {
            ragdollActivator.OnRagdollActivated -= PlayCrashSound;
        }

        private void PlayCrashSound()
        {
            AudioClip clip = crashSounds[Random.Range(0, crashSounds.Count)];

            audioSource.pitch = Random.Range(minPitch, maxPitch);

            audioSource.PlayOneShot(clip);
        }
    }
}