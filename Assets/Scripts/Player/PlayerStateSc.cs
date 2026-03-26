using UnityEngine;

namespace Mototaxi.Player
{
    public static class PlayerStateSc
    {
        [SerializeField] private static bool _isOnTrip = false;
        public static bool IsOnTrip => _isOnTrip;
        public static void StartTrip() => _isOnTrip = true;
        public static void EndTrip() => _isOnTrip = false;
    }
}