using UnityEngine;

namespace Mototaxi.Passenger
{
    public class BikePassengerSc : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void SetPassenger(PassengerDataSO data)
        {
            if (data.Mesh != null)
            {
                _meshRenderer.sharedMesh = data.Mesh;
                gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("[BikePassengerSc] Attempting to set a passenger without a valid Mesh.");
            }
        }

        public void Clear()
        {
            gameObject.SetActive(false);
            _meshRenderer.sharedMesh = null;
        }
    }
}