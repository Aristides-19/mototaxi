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
        [SerializeField] private CompassSc _compass;
        [SerializeField] private GameObject _pointMarker;

        private RoadPassengerSc _currentNearbyPassenger;
        private bool _isOnTrip = false;
        private PointSc _currentDestination;

        private void Awake()
        {
            _pointMarker.SetActive(false);
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

            _pointMarker.transform.position = _currentDestination.Position;
            _pointMarker.SetActive(true);

            _compass.SetDestination(_currentDestination.transform);

            _currentNearbyPassenger.Despawn();
            _currentNearbyPassenger = null;
        }

        public void CompleteTrip()
        {
            _isOnTrip = false;
            _bikePassenger.Clear();

            _compass.ClearDestination();
            _pointMarker.SetActive(false);

            _currentDestination = null;
        }
    }
}