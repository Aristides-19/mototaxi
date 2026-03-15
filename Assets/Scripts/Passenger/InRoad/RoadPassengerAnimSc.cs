using UnityEngine;
using Mototaxi.Core;
using Mototaxi.Utils;

namespace Mototaxi.Passenger
{
    [RequireComponent(typeof(Animator))]
    public class RoadPassengerAnimSc : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameDataSO _gameData;

        private Animator _animator;
        private readonly int arrivedHash = Animator.StringToHash("BikerArrived");
        private readonly int _randomIdxHash = Animator.StringToHash("RandomIdx");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            SetRandomIndex();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (FunctionsSc.IsLayerInLayerMask(other.gameObject.layer, _gameData.PlayerLayer))
            {
                TriggerArrived();
            }
        }

        private void TriggerArrived()
        {
            _animator.SetTrigger(arrivedHash);
            SetRandomIndex();
        }

        private void SetRandomIndex()
        {
            int randomIdx = Random.Range(0, 4);
            _animator.SetInteger(_randomIdxHash, randomIdx);
        }
    }
}