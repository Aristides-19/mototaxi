using UnityEngine;

namespace Mototaxi.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbientSc : MonoBehaviour
    {
        [Header("Variation Settings")]
        [Tooltip("Minimum pitch value (0.9 - 1.0 recommended) to prevent repetition")]
        [SerializeField] private float minPitch = 0.95f;

        [Tooltip("Maximum pitch value (1.0 - 1.1 recommended)")]
        [SerializeField] private float maxPitch = 1.05f;

        [Tooltip("How fast the variation changes over time")]
        [SerializeField] private float variationSpeed = 0.2f;

        [Tooltip("Minimum volume multiplier based on the original volume")]
        [SerializeField] private float minVolumeMultiplier = 0.8f;

        [Tooltip("Maximum volume multiplier based on the original volume")]
        [SerializeField] private float maxVolumeMultiplier = 1.0f;

        private AudioSource ambientAudioSource;
        private float initialVolume;
        private float randomOffset;

        private void Awake()
        {
            ambientAudioSource = GetComponent<AudioSource>();
            initialVolume = ambientAudioSource.volume;
            randomOffset = Random.Range(0f, 100f);
        }

        private void Start()
        {
            ambientAudioSource.loop = true;

            ambientAudioSource.time = Random.Range(0f, ambientAudioSource.clip.length);

            ambientAudioSource.Play();
        }

        private void Update()
        {
            float time = Time.time * variationSpeed + randomOffset;

            float pitchNoise = Mathf.PerlinNoise(time, 0f);
            ambientAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, pitchNoise);

            float volumeNoise = Mathf.PerlinNoise(0f, time);
            ambientAudioSource.volume = initialVolume * Mathf.Lerp(minVolumeMultiplier, maxVolumeMultiplier, volumeNoise);
        }
    }
}