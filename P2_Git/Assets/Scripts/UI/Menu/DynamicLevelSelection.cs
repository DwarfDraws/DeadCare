using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DynamicLevelSelection : MonoBehaviour
{
    private void Awake()
    {
        foreach (var button in GetComponentsInChildren<LevelSelectionButton>())
        {
            button.OnButtonClicked += ButtonOnOnButtonClicked;
        }
    }

    private void ButtonOnOnButtonClicked(int buttonNumber)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + buttonNumber);
    }

}
