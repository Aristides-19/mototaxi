using UnityEngine;

namespace Mototaxi.Trips
{
    public class PointMarkerAnimSc : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private float _rotationSpeed = 100f;

        [Header("Scale Pulsing Settings")]
        [SerializeField] private float _pulseSpeed = 2f;
        [SerializeField] private float _minScale = 0.8f;
        [SerializeField] private float _maxScale = 1.2f;

        private Vector3 _initialScale;

        private void Start()
        {
            _initialScale = transform.localScale;
        }

        private void Update()
        {
            ApplyRotation();
            ApplyPulsing();
        }

        private void ApplyRotation()
        {
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.Self);
        }

        private void ApplyPulsing()
        {
            float sinValue = Mathf.Sin(Time.time * _pulseSpeed);
            float normalizedSin = (sinValue + 1f) / 2f;

            float currentScaleMult = Mathf.Lerp(_minScale, _maxScale, normalizedSin);
            transform.localScale = _initialScale * currentScaleMult;
        }
    }
}