using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Script : MonoBehaviour
{
    [SerializeField] Transform text3d;
    Vector3 text2D;
    [SerializeField] Camera cam;
    [SerializeField] Slider slider;
    Canvas canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = this.GetComponent<Canvas>();
        text2D = cam.WorldToScreenPoint(text3d.position);
        slider.GetComponent<RectTransform>().anchoredPosition3D = text2D;
    }

    public void UpdateSlider(float timer){
        slider.value = timer;
    }
}
