using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

namespace ArcadeBP_Pro
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Setup")]
        [Tooltip("Array of Cinemachine cameras (Unity 6 / Cinemachine 3.x style).")]
        public CinemachineCamera[] cameras;

        [Tooltip("Reference to the bike controller.")]
        public ArcadeBikeControllerPro bikeController;

        [Header("Speed Effects")]
        [Tooltip("Speed threshold above which the camera shake effect starts.")]
        public float speedThresholdForShake = 50f;

        [Tooltip("Amplitude of the camera shake effect.")]
        public float shakeAmplitude = 1.2f;

        [Tooltip("Frequency of the camera shake effect.")]
        public float shakeFrequency = 2.0f;

        [Header("Controls")]
        public InputActionReference switchCameraAction;

        [Header("Field of View")]
        public float minFOV = 60f;
        public float maxFOV = 80f;
        public float FOV_smoother = 5f;

        private CinemachineBasicMultiChannelPerlin[] cameraNoise;
        private int currentCameraIndex = 0;
        private bool isShaking = false;
        private float smoothFOV = 60f;

        private Transform[] initialCameraFollowTargets;
        private Transform[] initialCameraLookAtTargets;

        private void Awake()
        {
            transform.SetParent(null);
            smoothFOV = minFOV;

            string bikeName = bikeController != null ? RemovePrefix(bikeController.name, "ABP_Pro") : "Unknown";
            gameObject.name = $"CameraController_{bikeName}";
        }

        void Start()
        {
            cameraNoise = new CinemachineBasicMultiChannelPerlin[cameras.Length];
            initialCameraFollowTargets = new Transform[cameras.Length];
            initialCameraLookAtTargets = new Transform[cameras.Length];

            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].Priority = (i == currentCameraIndex) ? 10 : 0;

                cameraNoise[i] = cameras[i].GetComponent<CinemachineBasicMultiChannelPerlin>();

                initialCameraFollowTargets[i] = cameras[i].Follow;
                initialCameraLookAtTargets[i] = cameras[i].LookAt;
            }
        }

        void Update()
        {
            HandleInput();

            if (bikeController == null) return;

            float bikeSpeed = bikeController.localBikeVelocity.magnitude;

            // Manejo de Shake
            if (bikeSpeed > speedThresholdForShake && bikeController.isActiveAndEnabled)
            {
                UpdateShake(bikeSpeed);
            }
            else if (isShaking)
            {
                StopShake();
            }

            // Manejo de FOV
            UpdateFOV(bikeSpeed);
        }

        private void HandleInput()
        {
            if (switchCameraAction.action.WasPressedThisFrame())
            {
                SwitchCamera();
            }
        }

        void SwitchCamera()
        {
            // En Cinemachine 3, simplemente bajamos la prioridad de la actual y subimos la nueva
            cameras[currentCameraIndex].Priority = 0;
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            cameras[currentCameraIndex].Priority = 10;

            StopShake();
        }

        void UpdateShake(float bikeSpeed)
        {
            isShaking = true;
            var currentNoise = cameraNoise[currentCameraIndex];

            if (currentNoise != null)
            {
                // Usamos la velocidad máxima configurada en el script de la moto
                float maxSpeed = bikeController.bikeSettings.maxSpeed;
                float t = Mathf.InverseLerp(speedThresholdForShake, maxSpeed, bikeSpeed);

                currentNoise.AmplitudeGain = Mathf.Lerp(0, shakeAmplitude, t);
                currentNoise.FrequencyGain = Mathf.Lerp(0, shakeFrequency, t);
            }
        }

        void StopShake()
        {
            isShaking = false;
            foreach (var noise in cameraNoise)
            {
                if (noise != null)
                {
                    noise.AmplitudeGain = 0f;
                    noise.FrequencyGain = 0f;
                }
            }
        }

        void UpdateFOV(float bikeSpeed)
        {
            float maxSpeed = bikeController.bikeSettings.maxSpeed;
            float t = Mathf.InverseLerp(0, maxSpeed, bikeSpeed);
            float targetFOV = Mathf.Lerp(minFOV, maxFOV, t);

            smoothFOV = Mathf.Lerp(smoothFOV, targetFOV, Time.deltaTime * FOV_smoother);

            var lens = cameras[currentCameraIndex].Lens;
            lens.FieldOfView = smoothFOV;
            cameras[currentCameraIndex].Lens = lens;
        }

        public void SetCameratarget(Transform followTarget, Transform lookAtTarget)
        {
            foreach (var cam in cameras)
            {
                cam.Follow = followTarget;
                cam.LookAt = lookAtTarget;
            }
            StopShake();
        }

        public void ResetCameraTarget()
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].Follow = initialCameraFollowTargets[i];
                cameras[i].LookAt = initialCameraLookAtTargets[i];
            }
            StopShake();
        }

        private string RemovePrefix(string originalName, string prefix)
        {
            return originalName.StartsWith(prefix) ? originalName.Substring(prefix.Length) : originalName;
        }
    }
}