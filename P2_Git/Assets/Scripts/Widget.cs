using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Widget : MonoBehaviour
{

    Slider slider;

    private void Start() {
        slider = this.gameObject.GetComponent<Slider>();
    }

    public void SetTimer(float time){
        slider.maxValue = time;
    }

    public void UpdateTimer(float timer){
        slider.value = timer;
    }
}
