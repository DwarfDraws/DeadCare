using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Script : MonoBehaviour
{
    [SerializeField] Raycast raycast;
    [SerializeField] Object_moveHandler obj_moveHandler;
    [SerializeField] Camera cam;
    [SerializeField] Slider slider_prefab;
    [SerializeField] GameObject pref_consumable;
    Canvas canvas;
    Object_attributes pref_consumable_attributes;
    public bool btnConsumable;
    float pref_consumable_localScaleX, pref_consumable_localScaleZ;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = this.GetComponent<Canvas>();
        pref_consumable_attributes = pref_consumable.GetComponent<Object_attributes>();
        pref_consumable_localScaleX = pref_consumable.transform.localScale.x;
        pref_consumable_localScaleZ = pref_consumable.transform.localScale.z;
    }

    public Widget InstantiateWidget(Vector3 widget_worldPos, float time, Color color)
    {
        Slider widget_instance = Slider.Instantiate(slider_prefab, widget_worldPos, Quaternion.identity, canvas.transform);
        widget_instance.maxValue = time;
        
        Image fillColor_image = widget_instance.gameObject.transform.GetChild(1).GetChild(0).transform.GetComponent<Image>();
        fillColor_image.color = color;

        RectTransform newWidget_transform = widget_instance.GetComponent<RectTransform>();
        
        Vector3 widget_screenPos = cam.WorldToScreenPoint(widget_worldPos);
        newWidget_transform.anchoredPosition3D = widget_screenPos;
        newWidget_transform.rotation = cam.transform.rotation;
        
        Widget widget = widget_instance.GetComponent<Widget>();
        return widget;
    }


    public void SetButton_Consumable(bool onOff)
    {
        btnConsumable = onOff;
    }

    public bool isBtnPressed_Consumable()
    {
        return btnConsumable;
    }

    public void InstantiateConsumable()
    {
        Vector3 instantiate_pos = raycast.GetMousePos3D();
        obj_moveHandler.SetObject(pref_consumable_attributes, pref_consumable_localScaleX, pref_consumable_localScaleZ);
        
        if(!obj_moveHandler.IsObjectAtEdge(instantiate_pos)){
            
            GameObject consumableInst = Instantiate(pref_consumable, raycast.GetMousePos3D(), Quaternion.identity);
                        
        }
    }
}
