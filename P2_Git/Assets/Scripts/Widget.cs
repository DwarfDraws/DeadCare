using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Widget : MonoBehaviour
{

    Slider slider;

    private void Start() 
    {
        slider = this.gameObject.GetComponent<Slider>();
    }

    public void SetTimer(float time)
    {
        slider.maxValue = time;
    }

    public void UpdateWidget(float timer)
    {
        slider.value = timer;
    }
}
