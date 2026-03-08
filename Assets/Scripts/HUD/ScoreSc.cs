using UnityEngine;
using TMPro;
using Mototaxi.Core;

namespace Mototaxi.UI
{
    public class ScoreSc : MonoBehaviour
    {
        private TextMeshProUGUI scoreText;

        void Awake()
        {
            // 1. Primero intenta buscar el componente de texto en el mismo objeto
            scoreText = GetComponent<TextMeshProUGUI>();

            // 2. Si no lo encuentra, lo busca adentro (en sus hijos)
            if (scoreText == null)
            {
                scoreText = GetComponentInChildren<TextMeshProUGUI>();
            }

            // 3. Aviso de seguridad por si de plano no hay ningún texto
            if (scoreText == null)
            {
                Debug.LogError("Falta TextMeshProUGUI: El script ScoreSc no encuentra el texto para actualizar los Bs.");
            }
        }

        void OnEnable()
        {
            // Nos suscribimos al evento del Manager
            ScoreManagerSc.OnScoreUpdated += UpdateScoreText;
        }

        void OnDisable()
        {
            // Nos desuscribimos para evitar errores de memoria
            ScoreManagerSc.OnScoreUpdated -= UpdateScoreText;
        }

        void Start()
        {
            // Al iniciar el juego, mostramos el dinero actual (ej. Bs. 0.00)
            UpdateScoreText(ScoreManagerSc.TotalScore);
        }

        private void UpdateScoreText(float newScore)
        {
            // La validación final que evita el error rojo de NullReferenceException
            if (scoreText != null)
            {
                scoreText.text = $"Bs. {newScore:F2}";
            }
        }
    }
}