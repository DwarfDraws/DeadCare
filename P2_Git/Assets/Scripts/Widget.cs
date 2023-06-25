using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Widget : MonoBehaviour
{

    Image img;

    private void Start() 
    {
        img = GetComponent<Image>();
    }

    public void SetTimer(float time)
    {
        img.fillAmount = time;
    }

    public void UpdateWidget(float timer)
    {
        img.fillAmount = timer;
    }
}
