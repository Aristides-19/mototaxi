using UnityEngine;

namespace Mototaxi.Passenger
{
    [CreateAssetMenu(fileName = "PassengerData", menuName = "Mototaxi/Passenger/PassengerData", order = 1)]
    public class PassengerDataSO : ScriptableObject
    {
        [Header("Visuals")]
        [SerializeField] private Mesh _mesh;

        [Header("Personality")]
        [SerializeField] private string _passengerName;
        [SerializeField][Range(1, 100)] private float _patience = 100f;
        [SerializeField][Range(0.5f, 2f)] private float _fareMultiplier = 1f;

        [Header("Physics")]
        [SerializeField] private float _mass = 80f;

        public Mesh Mesh => _mesh;
        public string PassengerName => _passengerName;
        public float Patience => _patience;
        public float FareMultiplier => _fareMultiplier;
        public float Mass => _mass;
    }
}