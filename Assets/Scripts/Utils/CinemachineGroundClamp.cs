using UnityEngine;
using Unity.Cinemachine;

namespace Mototaxi.Utils
{
    /// <summary>
    /// A Cinemachine extension that prevents the camera from going below a certain Y height by dynamically detecting the ground.
    /// Works on uneven terrain using Raycast.
    /// </summary>
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("Cinemachine/Extensions/Cinemachine Ground Clamp")]
    public class CinemachineGroundClamp : CinemachineExtension
    {
        [Header("Settings")]
        [Tooltip("Layers considered as ground (Terrain, Ground, etc).")]
        public LayerMask groundLayers = 1;

        [Tooltip("The minimum height the camera will maintain above the ground impact point.")]
        public float minHeightFromGround = 0.5f;

        [Tooltip("Thickness of the check to avoid thin collisions.")]
        public float checkRadius = 0.2f;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state,
            float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                Vector3 pos = state.RawPosition;

                Vector3 rayOrigin = pos;
                rayOrigin.y = pos.y + 100f;

                if (Physics.SphereCast(rayOrigin, checkRadius, Vector3.down, out RaycastHit hit, 200f, groundLayers))
                {
                    float groundY = hit.point.y;
                    float targetMinY = groundY + minHeightFromGround;

                    if (pos.y < targetMinY)
                    {
                        pos.y = targetMinY;
                        state.RawPosition = pos;
                    }
                }
            }
        }
    }
}