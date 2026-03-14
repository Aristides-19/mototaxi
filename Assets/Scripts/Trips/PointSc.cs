using System;
using UnityEngine;

namespace Mototaxi.Trips
{
    public class PointSc : MonoBehaviour
    {
        [SerializeField] private PointType _spawnPointType = PointType.Both;
        public PointType SpawnPointType => _spawnPointType;

        [NonSerialized]
        public bool isOccupied = false;

        public Quaternion Rotation => transform.rotation;
        public Vector3 Position => transform.position;

        private void OnDrawGizmos()
        {
            Gizmos.color = isOccupied ? Color.red : GetGizmosColor();

            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.DrawRay(transform.position, transform.forward * 3f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = isOccupied ? Color.red : GetGizmosColor();

            Gizmos.DrawWireSphere(transform.position, 1.5f);
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