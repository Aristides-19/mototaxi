using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject HowToPlay;
    public GameObject mainMenu;
    public void Start()
    {
        mainMenu.SetActive(true);
        HowToPlay.SetActive(false);
    }
    public void HowToPlayPanel()
    {
        mainMenu.SetActive(false);
        HowToPlay.SetActive(true);
    }

    public void MainMenuPanel()
    
    {
        mainMenu.SetActive(true);
        HowToPlay.SetActive(false);
    }

  
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GreyBoxing");

    }
}
