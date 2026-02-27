using System.Collections.Generic;
using Mototaxi.Core;
using UnityEngine;

namespace Mototaxi.Mechanics
{
    [RequireComponent(typeof(Collider))]
    public class RoceMechanicSc : MonoBehaviour
    {
        [SerializeField] Rigidbody playerRigidbody;
        [SerializeField] GameDataSc GameData;
        private readonly HashSet<Collider> collidedTraffic = new();

        void Awake()
        {
            Collider collider = GetComponent<Collider>();
            collider.isTrigger = true;

            if (playerRigidbody == null)
            {
                Debug.LogError("Player Rigidbody reference is missing in RoceMechanicSc.");
            }
        }

        void OnTriggerEnter(Collider other)
        {
            Vector3 direction = other.bounds.center - transform.position;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, GameData.RoceSettings.DistanceThreshold, GameData.TrafficLayer))
            {
                // Just register a roce if the raycast hits the traffic collider and the player is moving fast enough
                if (hit.collider == other && !collidedTraffic.Contains(other) && playerRigidbody.linearVelocity.magnitude > GameData.RoceSettings.MinVelocity)
                {
                    Debug.DrawRay(transform.position, direction, Color.red);
                    collidedTraffic.Add(other);
                    ScoreManagerSc.AddScore(GameData.RoceSettings.ScoreMultiplier / Mathf.Max(hit.distance, 0.1f));
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            collidedTraffic.Remove(other);
        }
    }
}