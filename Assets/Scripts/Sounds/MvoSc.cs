using UnityEngine;

namespace Mototaxi.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class MvoSc : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Probability (0.0 - 1.0) that the sound will play when spawned.")]
        [Range(0f, 1f)]
        [SerializeField] private float playChance = 0.5f;

        [Header("Audio Variation")]
        [SerializeField] private float minPitch = 0.9f;
        [SerializeField] private float maxPitch = 1.1f;

        [Header("Shake Effect")]
        [Tooltip("Optional transform to shake. If empty a child with MeshRenderer or the current transform will be used.")]
        [SerializeField] private Transform shakeTarget;
        [Tooltip("Intensity of the shake based on audio bass.")]
        [SerializeField] private float shakeMultiplier = 0.05f;
        [Tooltip("Frequency range to analyze for bass (0-10 recommended for low freq).")]
        [SerializeField] private int bassSampleCount = 10;

        private AudioSource audioSource;
        private Vector3 initialLocalPos;
        private bool isShaking = false;
        private readonly float[] spectrumData = new float[256];

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            initialLocalPos = shakeTarget.localPosition;
        }

        private void OnEnable()
        {
            if (Random.value <= playChance)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.Play();
                isShaking = true;
            }
            else
            {
                audioSource.Stop();
                isShaking = false;
            }
        }

        private void Update()
        {
            if (!isShaking || !audioSource.isPlaying) return;

            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

            float bassLevel = 0f;
            for (int i = 0; i < Mathf.Min(bassSampleCount, spectrumData.Length); i++)
            {
                bassLevel += spectrumData[i];
            }
            bassLevel /= bassSampleCount;

            if (bassLevel > 0.001f)
            {
                Vector3 randomOffset = bassLevel * 100f * shakeMultiplier * Random.insideUnitSphere;

                shakeTarget.localPosition = Vector3.Lerp(shakeTarget.localPosition, initialLocalPos + randomOffset, Time.deltaTime * 20f);
            }
            else
            {
                shakeTarget.localPosition = Vector3.Lerp(shakeTarget.localPosition, initialLocalPos, Time.deltaTime * 10f);
            }
        }

        private void OnDisable()
        {
            audioSource.Stop();
            shakeTarget.localPosition = initialLocalPos;

        }
    }
}