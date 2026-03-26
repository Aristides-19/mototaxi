using UnityEngine;
using Mototaxi.Passenger;
using Mototaxi.Core;
using Mototaxi.Player;
using Mototaxi.Utils;
using Mototaxi.HUD;
using ArcadeBP_Pro;

namespace Mototaxi.Trips
{
    [RequireComponent(typeof(ArcadeBikeControllerPro))]
    public class TripManagerSc : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameDataSO _gameData;
        [SerializeField] private BikePassengerSc _bikePassenger;
        [SerializeField] private InputActionsSO _inputActions;
        [SerializeField] private CompassSc _compass;
        [SerializeField] private GameObject _pointMarker;

        private RoadPassengerSc _currentNearbyPassenger;
        private PointSc _currentDestination;

        private ArcadeBikeControllerPro _bikeController;

        private void Awake()
        {
            _pointMarker.SetActive(false);
            _bikeController = GetComponent<ArcadeBikeControllerPro>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!PlayerStateSc.IsOnTrip && FunctionsSc.IsLayerInLayerMask(other.gameObject.layer, _gameData.PedestriansLayer))
            {
                _currentNearbyPassenger = other.GetComponentInParent<RoadPassengerSc>();
                HintManagerSc.TriggerHint(HintType.PickUpPassenger, -1);
            }

            if (PlayerStateSc.IsOnTrip && _currentDestination != null)
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
                if (!PlayerStateSc.IsOnTrip) HintManagerSc.HideHint();

            }
        }

        private void Update()
        {
            if (!PlayerStateSc.IsOnTrip && _currentNearbyPassenger != null && _inputActions.InteractAction.action.WasPressedThisFrame())
            {
                StartTrip();
            }
        }

        private void StartTrip()
        {
            if (_currentNearbyPassenger == null) return;

            PlayerStateSc.StartTrip();
            TimeManagerSc.MarkTripStart();

            HintManagerSc.TriggerHint(HintType.TripStart);

            // Score Logic
            float pickupScore = _gameData.TripSettings.BasePickupScore * _currentNearbyPassenger.CurrentData.FareMultiplier;
            ScoreManagerSc.AddScore(pickupScore, ScoreSource.TripStart);

            _bikePassenger.SetPassenger(_currentNearbyPassenger.CurrentData);

            // Reduce player max speed based on passenger mass
            _bikeController.bikeSettings.maxSpeed -= _currentNearbyPassenger.CurrentData.Mass * _gameData.PassengerSettings.MaxKmLossPerMassUnit;

            // Destination
            _currentDestination = _currentNearbyPassenger.DestinationPoint;
            _pointMarker.transform.position = _currentDestination.Position;
            _pointMarker.SetActive(true);
            _compass.SetDestination(_currentDestination.transform);

            // Passenger InRoad
            _currentNearbyPassenger.Despawn();
            _currentNearbyPassenger = null;
        }

        private void CompleteTrip()
        {
            _bikeController.bikeSettings.maxSpeed += _bikePassenger.CurrentData.Mass * _gameData.PassengerSettings.MaxKmLossPerMassUnit;

            float tripDuration = TimeManagerSc.CurrentTripDuration;
            // Trip Score Logic: (Base + (Bonus / Duration)) * FareMultiplier
            float timeBonus = tripDuration > 0 ? _gameData.TripSettings.TimeBonusMultiplier / tripDuration : 0;
            ScoreManagerSc.AddScore((_gameData.TripSettings.BaseDropOffScore + timeBonus) * _bikePassenger.CurrentData.FareMultiplier, ScoreSource.TripEnd);

            PlayerStateSc.EndTrip();
            _bikePassenger.Clear();

            HintManagerSc.TriggerHint(HintType.TripEnd);

            _compass.ClearDestination();
            _pointMarker.SetActive(false);

            _currentDestination = null;
        }
    }
}