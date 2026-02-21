using UnityEngine;
// 1. IMPORTANTE: Añadimos esta línea para usar el nuevo sistema
using UnityEngine.InputSystem;

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
        // 2. Cambiamos la forma de detectar la tecla Escape
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Pausa == false)
            {
                pausaMenu.SetActive(true);
                Panel.SetActive(true);
                Pausa = true;

                // Tip extra: Detener el tiempo del juego
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                // Por si quieres que al presionar Escape otra vez se cierre
                Continuar();
            }
        }
    }
    public void Continuar()
    {
        pausaMenu.SetActive(false);
        Panel.SetActive(false);
        Pausa = false;
        Time.timeScale = 1f; // Reanudar el tiempo
    }
}
