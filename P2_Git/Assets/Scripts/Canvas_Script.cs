using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Canvas_Script : MonoBehaviour
{
    [SerializeField] Raycast raycast;
    [SerializeField] Object_moveHandler obj_moveHandler;
    GameObject inGameMenuCanvas;
    Canvas canvas;
    Object_attributes pref_consumable_attributes;

    [SerializeField] Camera cam;
    [SerializeField] Slider slider_prefab;
    [SerializeField] GameObject pref_consumable;

    public GameObject btn_move, btn_tape, btn_skipCountdown;
    public GameObject pnl_GameOver;
    [SerializeField] GameObject youWin, youLose;
    [SerializeField] TMP_Text txt_ChildrenCounter;
    [SerializeField] TMP_Text txt_Countdown;
    [SerializeField] TMP_Text txt_tapeCounter;
    GameObject[] allObstacles;

    [HideInInspector] float pref_consumable_localScaleX, pref_consumable_localScaleZ;
    [HideInInspector] public bool isMoveBtnPressed, isTapeBtnPressed;
    [HideInInspector] bool btnConsumable;


    string pref_Canvas = "pref_Canvas";
    string tag_obstacle = "obstacle";
    
    // Start is called before the first frame update
    void Start()
    {
        inGameMenuCanvas = GameObject.Find(pref_Canvas);
        canvas = inGameMenuCanvas.GetComponent<Canvas>();
        pref_consumable_attributes = pref_consumable.GetComponent<Object_attributes>();
        pref_consumable_localScaleX = pref_consumable.transform.localScale.x;
        pref_consumable_localScaleZ = pref_consumable.transform.localScale.z;

        allObstacles = GameObject.FindGameObjectsWithTag(tag_obstacle);
    }

    public Widget InstantiateWidget(Vector3 widget_worldPos, Color color)
    {

        Slider widget_instance = Slider.Instantiate(slider_prefab, widget_worldPos, Quaternion.identity, canvas.transform);
        
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
        
        if(!obj_moveHandler.IsObjectAtEdge(instantiate_pos))
        {
            Instantiate(pref_consumable, instantiate_pos, Quaternion.identity);               
        }
    }

    public void moveButtonPressed()
    {
        isMoveBtnPressed = !isMoveBtnPressed;
        
        if(isMoveBtnPressed)
        {
            if(isTapeBtnPressed)
            {
                isTapeBtnPressed = false;
                btn_tape.GetComponent<Image>().color = Color.white;
                SetTapeableHalosActive(false);
            }
            btn_move.GetComponent<Image>().color = Color.gray;
            SetMoveableHalosActive(true);  
        }
        else
        { 
            btn_move.GetComponent<Image>().color = Color.white;
            SetMoveableHalosActive(false);
        }
    }

    public void Deactivate_MoveButton()
    {
        isMoveBtnPressed = false;
        btn_move.GetComponent<Image>().color = Color.white;
    }

    public void SetMoveableHalosActive(bool isActive)
    {
        foreach(GameObject obstacle in allObstacles)
        {
            GameObject halo = obstacle.GetComponent<Object_attributes>().moveableHalo;
            if(halo != null) halo.SetActive(isActive);
        }        
    }

    public void SetTapeableHalosActive(bool isActive)
    {
        foreach(GameObject obstacle in allObstacles)
        {
            GameObject halo = obstacle.GetComponent<Object_attributes>().tapeableHalo;
            if(halo != null) halo.SetActive(isActive);
        }        
    }

    public void tapeButtonPressed()
    {
        isTapeBtnPressed = !isTapeBtnPressed;

        if(isTapeBtnPressed)
        {
            if(isMoveBtnPressed)
            {
                isMoveBtnPressed = false;
                btn_move.GetComponent<Image>().color = Color.white;
                SetMoveableHalosActive(false);
            }
            btn_tape.GetComponent<Image>().color = Color.gray;
            SetTapeableHalosActive(true);
        }
        else 
        {
            btn_tape.GetComponent<Image>().color = Color.white;
            SetTapeableHalosActive(false);
        }
    }

    public void Deactivate_TapeButton()
    {
        isTapeBtnPressed = false;
        btn_tape.GetComponent<Image>().color = Color.white;
    }


    public void SetCountdown_Txt(string text)
    {
        txt_Countdown.text = text;
    }

    public void SetChildrenCounter_Txt(string text)
    {
        txt_ChildrenCounter.text = text;
    }

    public void SetYouWin(bool isWin)
    {
        if(isWin) 
        {
            youLose.SetActive(false);
            youWin.SetActive(true);
        }
        else
        {
            youLose.SetActive(true);
            youWin.SetActive(false);
        }
    }

    public void SetTapeCounter_Txt(string text)
    {
        txt_tapeCounter.text = text;
    }

}
