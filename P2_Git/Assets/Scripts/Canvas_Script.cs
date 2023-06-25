using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Canvas_Script : MonoBehaviour
{
    SaveData saveData;

    [SerializeField] Raycast raycast;
    [SerializeField] Object_moveHandler obj_moveHandler;
    [SerializeField] Gameplay gameplay;

    GameObject inGameMenuCanvas;
    Canvas canvas;
    Object_attributes pref_consumable_attributes;

    [SerializeField] Camera cam;
    [SerializeField] GameObject widget_prefab;
    [SerializeField] GameObject pref_consumable;
    [SerializeField] Image[] stars_Images;
    [SerializeField] Sprite star_Filled;
    [SerializeField] Sprite star_Empty;


    public GameObject btn_move, btn_tape, btn_skipCountdown, btn_consumable;
    public GameObject pnl_GameOver;
    [SerializeField] GameObject youWin, youLose;
    [SerializeField] TMP_Text txt_ChildrenCounter;
    [SerializeField] TMP_Text txt_Countdown;
    [SerializeField] TMP_Text txt_tapeCounter;
    [SerializeField] TMP_Text txt_consumableCounter;
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
        GameObject widget_instance = Instantiate(widget_prefab, widget_worldPos, Quaternion.identity, canvas.transform);
        
        //Image fillColor_image = widget_instance.gameObject.transform.GetChild(1).GetChild(0).transform.GetComponent<Image>();
        //fillColor_image.color = color;

        RectTransform newWidget_transform = widget_instance.GetComponent<RectTransform>();
        Vector3 widget_screenPos = cam.WorldToScreenPoint(widget_worldPos);
        newWidget_transform.anchoredPosition3D = widget_screenPos;
        newWidget_transform.rotation = Quaternion.identity;
        
        Widget widget = widget_instance.GetComponent<Widget>();
        return widget;
    }

    public void SetButton_Consumable(bool onOff)
    {
        btnConsumable = onOff;
    }
    
    public void ActivateButton_Consumable(bool onOff)
    {
        btn_consumable.SetActive(onOff);
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
            gameplay.InstantiateConsumable(pref_consumable, instantiate_pos);            
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


    public void Deactivate_MoveButton()
    {
        isMoveBtnPressed = false;
        btn_move.GetComponent<Image>().color = Color.white;
    }
    public void Deactivate_TapeButton()
    {
        isTapeBtnPressed = false;
        btn_tape.GetComponent<Image>().color = Color.white;
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
            Debug.Log(obstacle.name);
            GameObject halo = obstacle.GetComponent<Object_attributes>().tapeableHalo;
            if(halo != null) halo.SetActive(isActive);
        }        
    }








    public void SetCountdown_Txt(string text, float size)
    {
        txt_Countdown.text = text;
        txt_Countdown.fontSize = size;
    }

    public float GetCountdown_Txt_Size()
    {
        if(txt_Countdown != null) return txt_Countdown.fontSize;
        else return 0;
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

    public void SetConsumableCounter_Txt(string text)
    {
        txt_consumableCounter.text = text;
    }


    public void SetStarImages(int starCount)
    {
        switch (starCount)
        {
            case 0:
                foreach(Image img in stars_Images) img.sprite = star_Empty;
                break;
            case 1: 
                stars_Images[0].sprite = star_Filled;
                stars_Images[1].sprite = star_Empty;
                stars_Images[2].sprite = star_Empty;
                break;
            case 2:
                stars_Images[0].sprite = star_Filled;
                stars_Images[1].sprite = star_Filled;
                stars_Images[2].sprite = star_Empty;
                break;
            case 3:
                foreach(Image img in stars_Images) img.sprite = star_Filled;
                break;
        }
    }

}
