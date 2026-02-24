using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // ¡Librería obligatoria para cambiar de escenas!

public class PausaMenu : MonoBehaviour
{
    public GameObject pausaMenu;
    public GameObject Panel;
    public bool Pausa = false;

    void Start()
    {
        pausaMenu.SetActive(false);
        Panel.SetActive(false);
    }

    void Update()
    {
        // Detectar la tecla Escape con el nuevo Input System
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Pausa == false)
            {
                pausaMenu.SetActive(true);
                Panel.SetActive(true);
                Pausa = true;

                // Detener el tiempo del juego
                Time.timeScale = 0f;
                // Mostrar el cursor para poder hacer clic en los botones
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                // Si ya está pausado y presionas Escape, reanuda el juego
                Continuar();
            }
        }
    }

    // Función para el botón REANUDAR
    public void Continuar()
    {
        pausaMenu.SetActive(false);
        Panel.SetActive(false);
        Pausa = false;

        // Reanudar el tiempo
        Time.timeScale = 1f;

        // Ocultar el cursor y bloquearlo en el centro para seguir manejando
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // --- NUEVA FUNCIÓN PARA EL BOTÓN SALIR ---
    public void SalirAlMenu()
    {
        // 1. Reanudamos el tiempo antes de salir (vital para que el menú no esté congelado)
        Time.timeScale = 1f;

        // 2. Cargamos la escena. Asegúrate de que se llame EXACTAMENTE "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }
}