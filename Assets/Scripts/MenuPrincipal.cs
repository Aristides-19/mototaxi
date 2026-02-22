using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Cajas para arrastrar tus paneles desde el Inspector
    public GameObject panelMenuPrincipal;
    public GameObject panelComoJugar;

    public void EmpezarJuego()
    {
        SceneManager.LoadScene("GreyBoxing");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del simulador...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void AbrirComoJugar()
    {
        panelMenuPrincipal.SetActive(false); // Apaga el menú principal
        panelComoJugar.SetActive(true);      // Prende las instrucciones
    }

    public void CerrarComoJugar()
    {
        panelComoJugar.SetActive(false);     // Apaga las instrucciones
        panelMenuPrincipal.SetActive(true);  // Prende el menú principal
    }
}