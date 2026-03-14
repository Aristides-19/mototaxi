using System;
using UnityEngine;

namespace Mototaxi.Trips
{
    [ExecuteInEditMode]
    public class PointSc : MonoBehaviour
    {
        [SerializeField] private PointType _spawnPointType = PointType.Both;
        public PointType SpawnPointType => _spawnPointType;

        [Header("Editor Tools")]
        [SerializeField] private bool _autoSnap = true;
        [SerializeField] private LayerMask _roadLayer = 1 << 8;
        [SerializeField] private float _rayDistance = 25f;
        [SerializeField] private float _offsetFromGround = 0.05f;

        [NonSerialized]
        public bool isOccupied = false;

        public Quaternion Rotation => transform.rotation;
        public Vector3 Position => transform.position;

        private void Update()
        {
            if (!Application.isPlaying && _autoSnap && transform.hasChanged)
            {
                SnapToGround();
                transform.hasChanged = false;
            }
        }

        [ContextMenu("Snap to Ground")]
        public void SnapToGround()
        {
            Vector3 origin = transform.position + Vector3.up * (_rayDistance / 2f);
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, _rayDistance, _roadLayer))
            {
                transform.position = hit.point + Vector3.up * _offsetFromGround;

                Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
                if (projectedForward.sqrMagnitude > 0.0001f)
                {
                    transform.rotation = Quaternion.LookRotation(projectedForward, hit.normal);
                }

#if UNITY_EDITOR
                if (!Application.isPlaying)
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = isOccupied ? Color.red : GetGizmosColor();

            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.DrawRay(transform.position, transform.forward * 3f);

            if (!Application.isPlaying && !_autoSnap)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _rayDistance);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 1.2f);
        }

        private Color GetGizmosColor()
        {
            return SpawnPointType switch
            {
                PointType.SpawnOnly => Color.green,
                PointType.DestinationOnly => Color.blue,
                PointType.Both => Color.yellow,
                _ => Color.white
            };
        }
    }

    public enum PointType
    {
        SpawnOnly,
        DestinationOnly,
        Both
    }
}