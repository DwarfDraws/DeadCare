using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGamePauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObject;
    public GameObject inGameMenuObject;
    public AudioSource click;
    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Time.timeScale = 1;
            click.Play();
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

    public void nextLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
