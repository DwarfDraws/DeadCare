using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Target : MonoBehaviour
{
    Widget widget;
    Canvas_Script canvas;
    public Object_attributes attachedObject;
    public Animation_Script attachedObject_Animation;

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
        if(attachedObject != null && attachedObject.GetComponent<Animation_Script>() != null) attachedObject_Animation = attachedObject.GetComponent<Animation_Script>();

        timer = 1.0f;
        timerDown = true;
        if(attachedObject_Animation != null) attachedObject_Animation.SetAnimationSpeed(waitTime_seconds);
    }

    public void Animate_AttachedObject()
    {
        if(!isTaped && attachedObject_Animation != null) attachedObject_Animation.PlayAnimation(true);
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
            //Debug.Log(timer);

            if (timer > 1.0f)
            { 
                child.Reset();
                Destroy(widget.gameObject);
                attachedObject_Animation.PlayAnimation(false);
                ToggleDown(true);
                ResetTimer();
            }
        }
        
        if(!isWaitTarget && widget != null) widget.UpdateWidget(timer);
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

    public void DestroyWidget()
    {
        if(widget != null) Destroy(widget.gameObject);
        Debug.Log("destroywidget");
    }

    void ResetTimer(){
        timer = 1.0f;
    }
}
