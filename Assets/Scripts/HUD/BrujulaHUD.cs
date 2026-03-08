using UnityEngine;
using UnityEngine.UI; // Necesario para controlar elementos de la Interfaz

public class BrujulaHUD : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("El transform de la moto que va a girar")]
    public Transform jugador;

    [Tooltip("El componente Raw Image de la cinta infinita")]
    public RawImage cintaTextura;

    void Update()
    {
        // Una pequeña validación de seguridad por si olvidamos asignar algo
        if (jugador != null && cintaTextura != null)
        {
            // 1. Obtenemos hacia dónde está mirando la moto en grados (de 0 a 360)
            float gradosY = jugador.eulerAngles.y;

            // 2. Normalizamos ese valor a un porcentaje entre 0 y 1 (que es lo que lee la textura)
            // Sumamos 180 para que el "0" de la moto (Sur) coincida con el "180" de la brújula
            // El signo menos delante del paréntesis invierte el giro si iba al revés
            float posicionUV = (gradosY + 180f) / 360f;

            // 3. Desplazamos la textura horizontalmente manteniendo su tamaño original
            cintaTextura.uvRect = new Rect(posicionUV, 0f, 1f, 1f);
        }
    }
}