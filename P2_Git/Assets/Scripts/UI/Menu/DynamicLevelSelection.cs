using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DynamicLevelSelection : MonoBehaviour
{
  public AudioSource MenuClickSound;
    private void Awake()
    {
        
        foreach (var button in GetComponentsInChildren<LevelSelectionButton>())
        {
            button.OnButtonClicked += ButtonOnOnButtonClicked;
        }
    }

    private void ButtonOnOnButtonClicked(int buttonNumber)
    {
        MenuClickSound.Play();
        if(buttonNumber >= 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + buttonNumber);
        }
       
    }

}
