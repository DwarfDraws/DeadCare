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
    [SerializeField] GameObject btn_move, btn_tape;
    Canvas canvas;
    Object_attributes pref_consumable_attributes;
    public bool btnConsumable;
    public bool isMoveBtnPressed, isTapeBtnPressed;
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

    public void moveButtonPressed(){
        isMoveBtnPressed = !isMoveBtnPressed;

        if(isMoveBtnPressed)
        {
            if(isTapeBtnPressed)
            {
                isTapeBtnPressed = false;
                btn_tape.GetComponent<Image>().color = Color.white;
            }
            btn_move.GetComponent<Image>().color = Color.gray;
        }
        else btn_move.GetComponent<Image>().color = Color.white;
    }

    public void tapeButtonPressed(){
        isTapeBtnPressed = !isTapeBtnPressed;

        if(isTapeBtnPressed)
        {
            if(isMoveBtnPressed)
            {
                isMoveBtnPressed = false;
                btn_move.GetComponent<Image>().color = Color.white;
            }
            btn_tape.GetComponent<Image>().color = Color.gray;
        }
        else btn_tape.GetComponent<Image>().color = Color.white;
    }
}
