using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public GameObject pauseMenuObject;
    public GameObject inGameMenuObject;
    public AudioSource click;
    void Update()
    {

        if (Input.GetKeyDown("escape"))
        {
            Time.timeScale = 0;
            click.Play();
            pauseMenuObject.SetActive(true);
            inGameMenuObject.SetActive(false);
        }
    }

   
}
