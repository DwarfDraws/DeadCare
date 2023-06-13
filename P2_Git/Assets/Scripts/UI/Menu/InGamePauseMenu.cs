using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGamePauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    public void PauseMenuMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PauseMenuQuitGame()
    {
        Application.Quit();
    }
    
}
