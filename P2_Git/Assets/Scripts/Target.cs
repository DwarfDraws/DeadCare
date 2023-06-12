using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Target : MonoBehaviour
{
    Widget widget;
    Canvas_Script canvas;
    public Object_attributes attachedObject;

    public float waitTime_seconds = 2.0f;   
    public bool isDeadly;
    public bool isWaitTarget;
    public bool isConsumable;
    public bool isOpen; 
    public bool isTargeted;
    bool isTaped;
    bool timerDown;
    float timer;
    


    private void Start() {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas_Script>();
        timer = 1.0f;
        timerDown = true;
    }

    public void Timer(Children child)
    {
        //timer goes down
        if(timerDown)
        {
            timer -= Time.deltaTime / waitTime_seconds;

            if(timer <= 0)
            {
                if(isTaped)
                {
                    isOpen = true;
                    isTargeted = false;
                    isTaped = false;
                    attachedObject.SetTapeActive(false);
                    child.Reset(); 
                }
                
                else if(isDeadly)
                {
                    isOpen = true;
                    isTargeted = false;
                    child.ChildDestroy();
                }

                else child.Reset();

                DestroyWidget();
                ResetTimer();
            }
        }

        //timer goes up
        else if(!timerDown)
        {
            timer += Time.deltaTime / waitTime_seconds;

            if (timer > 1.0f)
            { 
                child.Reset();

                Destroy(widget.gameObject);
                ToggleDown(true);
                ResetTimer();
            }
        }
        
        if(!isTaped && attachedObject != null) attachedObject.Animate(timer);

        if(!isWaitTarget) widget.UpdateWidget(timer);
        
    }

    public void SetTargetTaped()
    {
        isTaped = true;
    }

    public void SetTargetUntaped() //Added by Anastasia
    {
        isTaped = false;
    }

    public void ToggleDown(bool isDown) 
    { 
        if(isDown) timerDown = true;
        else timerDown = false;
    }

    public void InstantiateWidget(Vector3 widget_worldPos, Color color)
    {
        widget = canvas.InstantiateWidget(widget_worldPos, color);
        ResetTimer();
    }

    public void DestroyWidget(){
        if(widget != null) Destroy(widget.gameObject);

        Debug.Log("destroyed");
    }

    void ResetTimer(){
        timer = 1.0f;
    }
}
