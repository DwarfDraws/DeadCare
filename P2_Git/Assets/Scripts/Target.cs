using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Widget widget;
    Canvas_Script canvas;

    [SerializeField] GameObject attachedObject;

    public float waitTime_seconds = 2.0f;   
    public bool isDeadly;
    public bool isConsumable;
    public bool isOpen; 
    public bool isTargeted;
    bool isTaped;
    bool timerDown;
    float timer;

    private void Start() {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas_Script>();
        timer = waitTime_seconds;
        timerDown = true;
    }

    public void Timer(Children child)
    {
        //timer goes down
        if(timerDown)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                if(isTaped)
                {
                    isOpen = true;
                    isTargeted = false;
                    isTaped = false;
                    if(attachedObject != null) attachedObject.SetActive(false);
                    else Debug.Log("No Tape removed");
                    child.Reset(); 
                }
                
                else if(isDeadly)
                {
                    isOpen = true;
                    isTargeted = false;
                    child.ChildDestroy();
                }

                else child.Reset();

                Destroy(widget.gameObject);
                ResetTimer();
            }
        }

        //timer goes up
        else if(!timerDown)
        {
            timer += Time.deltaTime;

            if (timer > waitTime_seconds)
            { 
                child.Reset();

                Destroy(widget.gameObject);
                ToggleDown(true);
                ResetTimer();
            }
        }

        widget.UpdateWidget(timer);
    }

    public void SetTargetTaped()
    {
        isTaped = true;
    }

    public void ToggleDown(bool isDown) 
    { 
        if(isDown) timerDown = true;
        else timerDown = false;
    }

    public void InstantiateWidget(Vector3 widget_worldPos, float time, Color color)
    {
        widget = canvas.InstantiateWidget(widget_worldPos, waitTime_seconds, color);
        ResetTimer();
    }

    public void DestroyWidget(){
        if(widget != null) Destroy(widget.gameObject);
    }

    void ResetTimer(){
        timer = waitTime_seconds;
    }
}
