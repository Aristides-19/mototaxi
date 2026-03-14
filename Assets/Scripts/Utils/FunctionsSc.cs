using System;
using UnityEngine;

namespace Mototaxi.Utils
{
    public class FunctionsSc
    {
        public static void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
        {
            return (layerMask.value & (1 << layer)) != 0;
        }

        public static int GetLayerFromLayerMask(LayerMask layerMask)
        {
            return (int)Math.Log(layerMask.value, 2);
        }
    }
}
