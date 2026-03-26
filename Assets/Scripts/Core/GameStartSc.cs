using System.Collections;
using Mototaxi.HUD;
using UnityEngine;

namespace Mototaxi.Core
{
    public class GameStartSc : MonoBehaviour
    {
        private void Start()
        {
            HintManagerSc.TriggerHint(HintType.Start, 5f);
            StartCoroutine(StartGameRoutine());
        }

        private IEnumerator StartGameRoutine()
        {
            yield return new WaitForSeconds(5f);
            HintManagerSc.TriggerHint(HintType.StartTime, 5f);
        }
    }
}