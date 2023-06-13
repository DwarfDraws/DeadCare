using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGamePauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObject;
    public GameObject inGameMenuObject;
    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Time.timeScale = 1;
            inGameMenuObject.SetActive(true);
            pauseMenuObject.SetActive(false);
        }
    }
    public void PauseMenuMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PauseMenuQuitGame()
    {
        Application.Quit();
    }
    public void ResetTimescale()
    {
        Time.timeScale = 1;
    }
}
