using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Script : MonoBehaviour
{
    [SerializeField] Transform text3d;
    Vector3 widget_screenPos;
    [SerializeField] Camera cam;
    [SerializeField] Slider slider_prefab;
    Canvas canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = this.GetComponent<Canvas>();
    }

    public Widget InstantiateWidget(Vector3 widget_worldPos, float time, bool isDeadly)
    {
        Slider widget_instance = Slider.Instantiate(slider_prefab, widget_worldPos, Quaternion.identity, canvas.transform);
        widget_instance.maxValue = time;
        
        Image fillColor_image = widget_instance.gameObject.transform.GetChild(1).GetChild(0).transform.GetComponent<Image>();
        if(isDeadly) fillColor_image.color = Color.red;
        else fillColor_image.color = Color.green;

        RectTransform newWidget_transform = widget_instance.GetComponent<RectTransform>();
        
        widget_screenPos = cam.WorldToScreenPoint(widget_worldPos);
        newWidget_transform.anchoredPosition3D = widget_screenPos;
        newWidget_transform.rotation = cam.transform.rotation;
        
        Widget widget = widget_instance.GetComponent<Widget>();
        return widget;
    }
}
