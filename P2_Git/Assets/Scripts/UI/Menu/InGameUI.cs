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
            pauseMenuObject.SetActive(true);
            inGameMenuObject.SetActive(false);
        }
    }
}
