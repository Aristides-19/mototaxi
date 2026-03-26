using Mototaxi.Core;
using Mototaxi.Player;
using Mototaxi.Utils;
using UnityEngine;

namespace Mototaxi.Mechanics
{
    [RequireComponent(typeof(Collider))]
    public class RoceMechanicSc : MonoBehaviour
    {
        [SerializeField] Rigidbody playerRigidbody;
        [SerializeField] GameDataSO GameData;

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
            if (!PlayerStateSc.IsOnTrip) return;

            // Avoid processing if the collided object is not in the TrafficLayer
            if (!FunctionsSc.IsLayerInLayerMask(other.gameObject.layer, GameData.TrafficLayer)) return;

            // Use the collider closest point for accuracy
            Vector3 closestPoint = other.ClosestPoint(transform.position);
            Vector3 direction = closestPoint - transform.position;

            // Use Raycast instead of just trigger to ensure there is no wall or obstacle between the player and the traffic object
            if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, GameData.RoceSettings.DistanceThreshold, GameData.TrafficLayer))
            {
                if (hit.collider == other && playerRigidbody.linearVelocity.magnitude > GameData.RoceSettings.MinVelocity)
                {
                    ScoreManagerSc.AddScore(GameData.RoceSettings.ScoreMultiplier / Mathf.Max(hit.distance, 0.1f), ScoreSource.Roce);
                }
            }
        }
    }
}