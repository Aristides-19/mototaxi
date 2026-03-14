using Mototaxi.Trips;
using UnityEngine;

namespace Mototaxi.Passenger
{
    public class RoadPassengerSc : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        private PassengerPoolSO _pool;
        private PointSc _spawnPoint;
        private PointSc _destinationPoint;
        private PassengerDataSO _currentData;

        public PassengerDataSO CurrentData => _currentData;
        public PointSc DestinationPoint => _destinationPoint;

        public void Init(PassengerPoolSO pool, PassengerDataSO data, PointSc spawnPoint, PointSc destinationPoint)
        {
            _pool = pool;
            _currentData = data;
            _spawnPoint = spawnPoint;
            _destinationPoint = destinationPoint;

            ApplyData(data);

            transform.SetPositionAndRotation(spawnPoint.Position, spawnPoint.Rotation);
            _spawnPoint.isOccupied = true;
        }

        private void ApplyData(PassengerDataSO data)
        {
            if (data.Mesh != null)
            {
                _meshRenderer.sharedMesh = data.Mesh;
            }
            else
            {
                Debug.LogWarning("PassengerDataSO Mesh is null. Please assign a valid mesh to the PassengerDataSO.");
            }
        }

        public void Despawn()
        {
            _spawnPoint.isOccupied = false;
            _spawnPoint = null;
            _destinationPoint = null;
            _pool.ReturnPassenger(this);
        }
    }
}