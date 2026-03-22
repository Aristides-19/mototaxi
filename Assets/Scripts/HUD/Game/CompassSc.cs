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

        [Header("Pointer Settings")]
        [Tooltip("El RectTransform del icono que se moverá indicando el destino")]
        [SerializeField] RectTransform pointerUI;

        [Tooltip("Cuántos grados del mundo representa el ancho total de la brújula en pantalla. 180 es ideal.")]
        [SerializeField] float gradosVisibles = 180f;

        // Variables privadas para controlar la lógica
        private Transform currentDestination;
        private float halfCompassWidth;

        void Awake()
        {
            if (playerRotator == null) Debug.LogError("playerRotator is not assigned in the inspector.");
            if (compassTextureUV == null) Debug.LogError("compassTextureUV is not assigned in the inspector.");
            if (pointerUI == null) Debug.LogError("pointerUI is not assigned in the inspector.");

            // Calculamos la mitad del ancho de tu brújula para saber el límite izquierdo y derecho
            if (compassTextureUV != null)
            {
                halfCompassWidth = compassTextureUV.rectTransform.rect.width / 2f;
            }

            // Ocultamos el puntero al inicio del juego (solo aparece si hay misión)
            if (pointerUI != null) pointerUI.gameObject.SetActive(false);
        }

        void Update()
        {
            // --- 1. GIRA LA BRÚJULA (Tu código original) ---
            float gradosY = playerRotator.eulerAngles.y;
            float posicionUV = (gradosY + 180f) / 360f;
            compassTextureUV.uvRect = new Rect(posicionUV, 0f, 1f, 1f);

            // --- 2. MUEVE EL PUNTERO (Lo nuevo) ---
            if (currentDestination != null && pointerUI != null)
            {
                ActualizarPuntero();
            }
        }

        void ActualizarPuntero()
        {
            // Calculamos la dirección hacia el destino (ignorando la altura para que no se vuelva loco en subidas)
            Vector3 dirToDest = currentDestination.position - playerRotator.position;
            dirToDest.y = 0;

            // Obtenemos el ángulo entre el frente de la moto y el destino (nos dará entre -180 y 180 grados)
            float angleToDest = Vector3.SignedAngle(playerRotator.forward, dirToDest, Vector3.up);

            // Convertimos ese ángulo en una escala de -1 (izquierda) a 1 (derecha)
            float porcentaje = angleToDest / (gradosVisibles / 2f);

            // "Clampeamos" el valor. Esto hace que si el pasajero está detrás de ti, 
            // el puntero no se salga de la pantalla, sino que se quede pegado al borde indicando "date la vuelta".
            porcentaje = Mathf.Clamp(porcentaje, -1f, 1f);

            // Le aplicamos la nueva posición X al puntero (manteniendo su Y original)
            pointerUI.anchoredPosition = new Vector2(porcentaje * halfCompassWidth, pointerUI.anchoredPosition.y);
        }

        // --- FUNCIONES PÚBLICAS PARA LLAMAR DESDE OTROS SCRIPTS ---

        public void SetDestination(Transform newDest)
        {
            currentDestination = newDest;
            pointerUI.gameObject.SetActive(true);
        }

        public void ClearDestination()
        {
            currentDestination = null;
            pointerUI.gameObject.SetActive(false);
        }
    }
}