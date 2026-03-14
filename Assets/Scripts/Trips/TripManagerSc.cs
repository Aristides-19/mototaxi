using UnityEngine;
using Mototaxi.Passenger;
using Mototaxi.Core;
using Mototaxi.Player;
using Mototaxi.Utils;

namespace Mototaxi.Trips
{
    public class TripManagerSc : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameDataSO _gameData;
        [SerializeField] private BikePassengerSc _bikePassenger;
        [SerializeField] private InputActionsSO _inputActions;

        private PassengerController _currentNearbyPassenger;
        private bool _isOnTrip = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!_isOnTrip && FunctionsSc.IsLayerInLayerMask(other.gameObject.layer, _gameData.PedestriansLayer))
            {
                _currentNearbyPassenger = other.GetComponentInParent<PassengerController>();
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

            PointSc destination = _currentNearbyPassenger.DestinationPoint;

            _currentNearbyPassenger.Despawn();
            _currentNearbyPassenger = null;
        }

        public void CompleteTrip()
        {
            _isOnTrip = false;
            _bikePassenger.Clear();
        }
    }
}