using UnityEngine;
using Mototaxi.Passenger;
using Mototaxi.Core;
using Mototaxi.Player;
using Mototaxi.Utils;
using Mototaxi.HUD;

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

        // --- NUEVAS REFERENCIAS PARA EL HALO ---
        [Header("Mission Visuals")]
        [Tooltip("El GameObject del Halo (Beacon) que aparecerá en el destino")]
        [SerializeField] private GameObject missionHaloVisuals; // Arrastra tu prefab/objeto 'Mission_Beacon' aquí

        private RoadPassengerSc _currentNearbyPassenger;
        private bool _isOnTrip = false;
        private PointSc _currentDestination;

        private void Awake()
        {
            // --- NUEVO: Asegurarnos de que el Halo esté apagado al inicio del juego ---
            if (missionHaloVisuals != null)
            {
                missionHaloVisuals.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isOnTrip && FunctionsSc.IsLayerInLayerMask(other.gameObject.layer, _gameData.PedestriansLayer))
            {
                _currentNearbyPassenger = other.GetComponentInParent<RoadPassengerSc>();
            }

            if (_isOnTrip && _currentDestination != null)
            {
                PointSc puntoTocado = other.GetComponent<PointSc>();
                if (puntoTocado != null && puntoTocado == _currentDestination)
                {
                    CompleteTrip();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (FunctionsSc.IsLayerInLayerMask(other.gameObject.layer, _gameData.PedestriansLayer))
            {
                _currentNearbyPassenger = null;
            }
        }

        private void Update()
        {
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

            _currentDestination = _currentNearbyPassenger.DestinationPoint;

            // --- NUEVO: Activar y Teletransportar el Halo ---
            if (missionHaloVisuals != null && _currentDestination != null)
            {
                // Teletransportamos el Halo a la posición exacta del punto de destino
                // IMPORTANTE: Asegúrate de que el modelo del Halo en sí (su hijo) 
                // esté posicionado de forma que la base del cilindro toque el suelo (Y=0 respecto al padre).
                missionHaloVisuals.transform.position = _currentDestination.Position;

                // Rotamos el halo para que coincida con la orientación del punto (por si acaso)
                missionHaloVisuals.transform.rotation = _currentDestination.Rotation;

                // Encendemos el Halo visual
                missionHaloVisuals.SetActive(true);
            }

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
            _bikePassenger.Clear();

            if (_compass != null)
            {
                _compass.ClearDestination();
            }

            // --- NUEVO: Apagar el Halo visual ---
            if (missionHaloVisuals != null)
            {
                missionHaloVisuals.SetActive(false);
            }

            _currentDestination = null;

            Debug.Log("¡Viaje completado y Halo apagado!");
        }
    }
}