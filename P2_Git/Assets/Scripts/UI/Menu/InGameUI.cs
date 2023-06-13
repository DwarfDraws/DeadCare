using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public GameObject pauseMenuObject;
    public GameObject inGameMenuObject;
    void Update()
    {

        if (Input.GetKeyDown("escape"))
        {
            Time.timeScale = 0;
            pauseMenuObject.SetActive(true);
            inGameMenuObject.SetActive(false);
        }
    }
}
