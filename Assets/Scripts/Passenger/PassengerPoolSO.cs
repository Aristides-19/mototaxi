using Mototaxi.Trips;
using Mototaxi.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace Mototaxi.Passenger
{
    [CreateAssetMenu(fileName = "PassengerPool", menuName = "Mototaxi/Passenger/PassengerPool", order = 0)]
    public class PassengerPoolSO : ScriptableObject
    {
        [SerializeField] private PassengerController passengerBasePrefab;
        [SerializeField] private List<PassengerDataSO> availablePassengerData;
        private ObjectPoolSc<PassengerController> pool;

        public void Init(Transform container, int initialPassengers, int maxPassengers)
        {
            pool = new ObjectPoolSc<PassengerController>(passengerBasePrefab, container, initialPassengers, maxPassengers);
            pool.PreWarm(initialPassengers);
        }

        public PassengerController GetPassenger(PointSc spawnPoint, PointSc destinationPoint)
        {
            PassengerController passenger = pool.Get();
            PassengerDataSO data = availablePassengerData[Random.Range(0, availablePassengerData.Count)];

            passenger.Init(this, data, spawnPoint, destinationPoint);
            return passenger;
        }

        public void ReturnPassenger(PassengerController passenger)
        {
            pool.Release(passenger);
        }
    }
}