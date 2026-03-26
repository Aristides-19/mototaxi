using UnityEngine;
using ArcadeBP_Pro;
using System.Collections.Generic;

namespace Mototaxi.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class AlarmSc : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Reference to the RagdollActivator component.")]
        [SerializeField] private RagdollActivator ragdollActivator;

        [Header("Audio Settings")]
        [Tooltip("List of alarm sound effects to play randomly.")]
        [SerializeField] private List<AudioClip> alarmSounds;

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
            ragdollActivator.OnRagdollActivated += PlayAlarmSound;
        }

        private void OnDisable()
        {
            ragdollActivator.OnRagdollActivated -= PlayAlarmSound;
        }

        private void PlayAlarmSound()
        {
            AudioClip clip = alarmSounds[Random.Range(0, alarmSounds.Count)];

            audioSource.pitch = Random.Range(minPitch, maxPitch);

            audioSource.PlayOneShot(clip);
        }
    }
}