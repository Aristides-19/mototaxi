using UnityEngine;
using Unity.Cinemachine; // Namespace de Unity 6
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineCamera), typeof(CinemachineThirdPersonFollow))]
public class CameraLean : MonoBehaviour
{
    [Header("Configuración de Efecto")]
    [Tooltip("Cuánto se mueve la cámara hacia el interior de la curva (Metros)")]
    [SerializeField] float leanAmount = 2.0f;

    [Tooltip("Tiempo en segundos para completar el movimiento. Valores más altos = más suave (0.2 a 0.5 recomendado)")]
    [SerializeField] float smoothTime = 0.5f; // PRUEBA CON 0.3f

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
