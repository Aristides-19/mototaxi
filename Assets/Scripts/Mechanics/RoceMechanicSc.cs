using System.Collections;
using Mototaxi.Core;
using UnityEngine;

namespace Mototaxi.Mechanics
{
    [RequireComponent(typeof(Collider))]
    public class RoceMechanicSc : MonoBehaviour
    {
        [SerializeField] Rigidbody playerRigidbody;
        [SerializeField] GameDataSc GameData;

        void Awake()
        {
            Collider collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            // Verificamos si chocamos con un objeto que pertenece a la capa de Tráfico
            if (((1 << other.gameObject.layer) & GameData.TrafficLayer) == 0) return;

            Vector3 closestPoint = other.ClosestPoint(transform.position);
            Vector3 direction = closestPoint - transform.position;

            if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, GameData.RoceSettings.DistanceThreshold, GameData.TrafficLayer))
            {
                // Extraemos la velocidad actual de la moto
                float currentSpeed = playerRigidbody.linearVelocity.magnitude;

                if (hit.collider == other && currentSpeed > GameData.RoceSettings.MinVelocity)
                {
                    // 1. Calculamos los puntos tomando en cuenta la velocidad y la cercanía
                    // Fórmula: (Multiplicador * Velocidad) / Distancia
                    int pointsEarned = Mathf.FloorToInt((GameData.RoceSettings.ScoreMultiplier * currentSpeed) / Mathf.Max(hit.distance, 0.1f));

                    // 2. Sumamos al dinero total (Bs.)
                    ScoreManagerSc.AddScore(pointsEarned);

                    // 3. Mostramos el mensaje en el HUD de trucos
                    ScoreManagerSc.UpdateTrickUI("¡CERCA!", pointsEarned);

                    // 4. Iniciamos el temporizador para borrar el mensaje
                    StopAllCoroutines(); // Por si haces varios roces seguidos
                    StartCoroutine(HideTrickTextDelay());
                }
            }
        }

        // El temporizador que borra el texto después de 1.5 segundos
        private IEnumerator HideTrickTextDelay()
        {
            yield return new WaitForSeconds(1.5f);
            ScoreManagerSc.UpdateTrickUI("", 0); // Enviamos texto vacío para borrarlo
        }
    }
}