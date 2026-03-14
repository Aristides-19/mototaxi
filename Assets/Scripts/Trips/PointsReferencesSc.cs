using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mototaxi.Trips
{
    public class PointsReferencesSc : MonoBehaviour
    {
        [SerializeField] private PointSc[] spawnPoints;
        public PointSc[] SpawnPoints => spawnPoints;

        [SerializeField] private PointSc[] destinationPoints;
        public PointSc[] DestinationPoints => destinationPoints;

        [SerializeField] private PointSc[] bothPoints;
        public PointSc[] BothPoints => bothPoints;

        [ContextMenu("Fill Points")]
        private void FillSpawnPoints()
        {
            List<PointSc> points = GetComponentsInChildren<PointSc>().ToList();
            spawnPoints = points.Where(p => p.SpawnPointType == PointType.SpawnOnly || p.SpawnPointType == PointType.Both).ToArray();
            destinationPoints = points.Where(p => p.SpawnPointType == PointType.DestinationOnly || p.SpawnPointType == PointType.Both).ToArray();
            bothPoints = points.Where(p => p.SpawnPointType == PointType.Both).ToArray();
        }
    }
}