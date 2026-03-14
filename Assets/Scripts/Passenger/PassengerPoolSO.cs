using Mototaxi.Trips;
using Mototaxi.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace Mototaxi.Passenger
{
    [CreateAssetMenu(fileName = "PassengerPool", menuName = "Mototaxi/Passenger/PassengerPool", order = 0)]
    public class PassengerPoolSO : ScriptableObject
    {
        [SerializeField] private RoadPassengerSc passengerBasePrefab;
        [SerializeField] private List<PassengerDataSO> availablePassengerData;
        private ObjectPoolSc<RoadPassengerSc> pool;

        public void Init(Transform container, int initialPassengers, int maxPassengers)
        {
            pool = new ObjectPoolSc<RoadPassengerSc>(passengerBasePrefab, container, initialPassengers, maxPassengers);
            pool.PreWarm(initialPassengers);
        }

        public RoadPassengerSc GetPassenger(PointSc spawnPoint, PointSc destinationPoint)
        {
            RoadPassengerSc passenger = pool.Get();
            PassengerDataSO data = availablePassengerData[Random.Range(0, availablePassengerData.Count)];

            passenger.Init(this, data, spawnPoint, destinationPoint);
            return passenger;
        }

        public void ReturnPassenger(RoadPassengerSc passenger)
        {
            pool.Release(passenger);
        }
    }
}