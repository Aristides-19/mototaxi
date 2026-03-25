using UnityEngine;
using UnityEngine.UI;

namespace Mototaxi.HUD
{
    public class CompassSc : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Transform of the player or camera whose rotation will be used")]
        [SerializeField] Transform playerRotator;

        [Tooltip("Compass RawImage component to update the UV rect")]
        [SerializeField] RawImage compassTextureUV;

        [Header("Pointer Settings")]
        [Tooltip("UI element that points towards the destination")]
        [SerializeField] RectTransform pointerTransform;

        [Header("Compass Range")]
        [Tooltip("How many degrees of the texture are visible on screen? (e.g., 180 means North to South)")]
        [Range(0, 360)]
        [SerializeField] float visibleFOV = 360f;

        private Transform currentDestination;

        private float HalfContainerWidth
        {
            get
            {
                if (pointerTransform != null && pointerTransform.parent is RectTransform parentRect)
                {
                    return parentRect.rect.width / 2f;
                }
                return compassTextureUV != null ? compassTextureUV.rectTransform.rect.width / 2f : 0f;
            }
        }

        void Awake()
        {
            if (playerRotator == null) playerRotator = Camera.main.transform;
            if (compassTextureUV == null) Debug.LogError("compassTextureUV is not assigned.");
            if (pointerTransform == null) Debug.LogError("pointerTransform is not assigned.");

            pointerTransform.gameObject.SetActive(false);
        }

        void Update()
        {
            float playerRotationY = playerRotator.eulerAngles.y;
            float uvWidth = visibleFOV / 360f;


            float posicionUV = (playerRotationY / 360f) - (uvWidth / 2f);
            compassTextureUV.uvRect = new Rect(posicionUV, 0f, uvWidth, 1f);

            if (currentDestination != null) UpdatePointer();
        }

        void UpdatePointer()
        {
            Vector3 playerPos = playerRotator.position;
            Vector3 destPos = currentDestination.position;

            Vector3 forward = playerRotator.forward;
            forward.y = 0;

            Vector3 dirToDest = destPos - playerPos;
            dirToDest.y = 0;

            if (dirToDest.sqrMagnitude < 0.1f) return;

            float angleToDest = Vector3.SignedAngle(forward.normalized, dirToDest.normalized, Vector3.up);
            pointerTransform.anchoredPosition = new Vector2(Mathf.Clamp(angleToDest / (visibleFOV / 2f), -1f, 1f) * HalfContainerWidth, pointerTransform.anchoredPosition.y);
        }

        public void SetDestination(Transform newDest)
        {
            currentDestination = newDest;
            if (pointerTransform != null) pointerTransform.gameObject.SetActive(true);
        }

        public void ClearDestination()
        {
            currentDestination = null;
            if (pointerTransform != null) pointerTransform.gameObject.SetActive(false);
        }
    }
}