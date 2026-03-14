using UnityEngine;
using UnityEngine.UI;

namespace Mototaxi.HUD
{
    public class CompassSc : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Transform of the player or object whose rotation will be used to update the compass")]
        [SerializeField] Transform playerRotator;

        [Tooltip("Compass RawImage component to update the UV rect")]
        [SerializeField] RawImage compassTextureUV;

        void Awake()
        {
            if (playerRotator == null)
            {
                Debug.LogError("playerRotator is not assigned in the inspector.");
            }
            if (compassTextureUV == null)
            {
                Debug.LogError("compassTextureUV is not assigned in the inspector.");
            }
        }

        void Update()
        {
            float gradosY = playerRotator.eulerAngles.y;
            float posicionUV = (gradosY + 180f) / 360f;
            compassTextureUV.uvRect = new Rect(posicionUV, 0f, 1f, 1f);
        }
    }
}