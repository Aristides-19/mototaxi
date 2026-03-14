using UnityEngine;
using Mototaxi.Passenger;
using Mototaxi.Core;
using System.Linq;

namespace Mototaxi.Trips
{
    public class SpawnManagerSc : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameDataSO _gameData;
        [SerializeField] private PassengerPoolSO _passengerPool;
        [SerializeField] private PointsReferencesSc _pointsReferences;
        [SerializeField] private Transform _passengerContainer;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            PassengerData settings = _gameData.PassengerSettings;

            _passengerPool.Init(_passengerContainer, settings.InitialPassengers, settings.MaxPassengers);

            PopulateMap();
        }

        private void PopulateMap()
        {
            PassengerData settings = _gameData.PassengerSettings;

            int targetCount = Mathf.RoundToInt(_pointsReferences.SpawnPoints.Count() * settings.SpawnDensity);
            targetCount = Mathf.Min(targetCount, settings.MaxPassengers);

            var shuffledSpawns = _pointsReferences.SpawnPoints.OrderBy(x => Random.value).ToList();

            int currentSpawned = 0;
            for (int i = 0; i < shuffledSpawns.Count && currentSpawned < targetCount; i++)
            {
                PointSc spawnPoint = shuffledSpawns[i];

                PointSc destination = _pointsReferences.DestinationPoints
                    .Where(d => d != spawnPoint && Vector3.Distance(spawnPoint.Position, d.Position) >= settings.MinTripDistance)
                    .OrderBy(x => Random.value)
                    .FirstOrDefault();

                if (destination == null) destination = _pointsReferences.DestinationPoints
                                                        .FirstOrDefault(d => d != spawnPoint);

                _passengerPool.GetPassenger(spawnPoint, destination);
                currentSpawned++;
            }
        }
    }
}