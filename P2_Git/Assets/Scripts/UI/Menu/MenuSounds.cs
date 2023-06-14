using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    public AudioSource buttonClick;
    public AudioSource buttonClickBack;
    public void ButtonClick()
    {
        buttonClick.Play();
    }
    public void ButtonClickBack()
    {
        buttonClickBack.Play();
    }
}
