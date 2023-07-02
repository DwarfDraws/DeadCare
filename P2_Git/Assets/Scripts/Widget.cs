using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Widget : MonoBehaviour
{
    public Color red;
    public Color yellow;
    public Color green;
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
        img.color = green;
        
        if (timer <= 0.5f)
        {
            img.color = yellow;

            if (timer <= 0.25f)
            {
                img.color = red;
            }
        }
    }
}
