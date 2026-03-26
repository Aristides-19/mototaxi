using ArcadeBP_Pro;
using Mototaxi.Core;
using Mototaxi.Utils;
using UnityEngine;

namespace Mototaxi.Road
{
    [RequireComponent(typeof(Rigidbody))]
    public class LimitSc : MonoBehaviour
    {
        [SerializeField] private GameDataSO _gameData;
        [SerializeField] private RagdollActivator _ragdollActivator;

        private void OnTriggerEnter(Collider other)
        {
            if (FunctionsSc.IsLayerInLayerMask(other.gameObject.layer, _gameData.PlayerLayer))
            {
                _ragdollActivator.ForceActivateRagdoll();
            }
        }
    }
}