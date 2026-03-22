using UnityEngine;
using Mototaxi.Passenger;
using Mototaxi.Core;
using Mototaxi.Player;
using Mototaxi.Utils;
using Mototaxi.HUD; // Conexión a tu brújula

namespace Mototaxi.Trips
{
    public class TripManagerSc : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameDataSO _gameData;
        [SerializeField] private BikePassengerSc _bikePassenger;
        [SerializeField] private InputActionsSO _inputActions;

        [Header("UI & Navigation")]
        [SerializeField] private CompassSc _compass;

        private RoadPassengerSc _currentNearbyPassenger;
        private bool _isOnTrip = false;

        // Guardamos cuál es el punto al que tenemos que ir
        private PointSc _currentDestination;

        private void OnTriggerEnter(Collider other)
        {
            // 1. Detectar si nos acercamos a un pasajero en la calle
            if (!_isOnTrip && FunctionsSc.IsLayerInLayerMask(other.gameObject.layer, _gameData.PedestriansLayer))
            {
                _currentNearbyPassenger = other.GetComponentInParent<RoadPassengerSc>();
            }

            // 2. LA MAGIA AUTOMÁTICA: Detectar si llegamos al punto de destino
            if (_isOnTrip && _currentDestination != null)
            {
                PointSc puntoTocado = other.GetComponent<PointSc>();

                // Si el Trigger que acabamos de tocar es exactamente nuestro destino...
                if (puntoTocado != null && puntoTocado == _currentDestination)
                {
                    // ¡Completamos el viaje al instante, sin presionar nada!
                    CompleteTrip();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Detectar si nos alejamos del pasajero de la calle sin recogerlo
            if (FunctionsSc.IsLayerInLayerMask(other.gameObject.layer, _gameData.PedestriansLayer))
            {
                _currentNearbyPassenger = null;
            }
        }

        private void Update()
        {
            // Para INICIAR el viaje SÍ necesitamos presionar el botón de interactuar
            if (!_isOnTrip && _currentNearbyPassenger != null && _inputActions.InteractAction.action.WasPressedThisFrame())
            {
                StartTrip();
            }
        }

        private void StartTrip()
        {
            if (_currentNearbyPassenger == null) return;

            _isOnTrip = true;

            _bikePassenger.SetPassenger(_currentNearbyPassenger.CurrentData);

            // Guardamos a dónde tenemos que ir
            _currentDestination = _currentNearbyPassenger.DestinationPoint;

            // Le decimos a la brújula que nos guíe
            if (_compass != null && _currentDestination != null)
            {
                _compass.SetDestination(_currentDestination.transform);
            }

            _currentNearbyPassenger.Despawn();
            _currentNearbyPassenger = null;
        }

        public void CompleteTrip()
        {
            _isOnTrip = false;
            _bikePassenger.Clear(); // Esto desaparece a la chica de la moto

            // Apagamos la brújula
            if (_compass != null)
            {
                _compass.ClearDestination();
            }

            // Borramos el destino actual para que estemos listos para otra carrera
            _currentDestination = null;

            Debug.Log("¡Llegaste! El pasajero se ha bajado automáticamente.");
        }
    }
}