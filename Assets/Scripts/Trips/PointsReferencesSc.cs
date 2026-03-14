using UnityEngine;

namespace Mototaxi.Trips
{
    public class PointsReferencesSc : MonoBehaviour
    {
        [SerializeField] private PointSc[] spawnPoints;
        public PointSc[] SpawnPoints => spawnPoints;

        [ContextMenu("Fill Spawn Points")]
        private void FillSpawnPoints()
        {
            spawnPoints = GetComponentsInChildren<PointSc>();
        }
    }
}