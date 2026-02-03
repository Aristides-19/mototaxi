using UnityEngine;
using Unity.Cinemachine; // Namespace de Unity 6
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineCamera), typeof(CinemachineThirdPersonFollow))]
public class CameraLean : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How much the camera moves X shoulder offset during a turn (Meters)")]
    [SerializeField] float leanAmount = 1.5f;

    [Tooltip("Time in seconds to complete the movement. Higher values = smoother")]
    [SerializeField] float smoothTime = 1f;

    CinemachineCamera cmCamera;
    CinemachineThirdPersonFollow followComponent;
    InputAction moveInput;
    float initialOffsetX;
    float currentVelocity;

    void Awake()
    {
        moveInput = InputSystem.actions.FindAction("Move");
        cmCamera = GetComponent<CinemachineCamera>();

        followComponent = cmCamera.GetComponent<CinemachineThirdPersonFollow>();
        initialOffsetX = followComponent.ShoulderOffset.x;
    }

    void Update()
    {
        float turnInput = moveInput.ReadValue<Vector2>().x;
        float targetOffsetX = initialOffsetX + (turnInput * leanAmount);

        Vector3 currentOffset = followComponent.ShoulderOffset;

        // CAMBIO MÁGICO: SmoothDamp
        // En lugar de moverlo linealmente, esto crea una curva de velocidad suave
        currentOffset.x = Mathf.SmoothDamp(currentOffset.x, targetOffsetX, ref currentVelocity, smoothTime);
        // currentOffset.x = Mathf.Lerp(currentOffset.x, targetOffsetX, Time.deltaTime * 2f);
        followComponent.ShoulderOffset = currentOffset;
    }
}
