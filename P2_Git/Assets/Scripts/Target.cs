using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Target : MonoBehaviour
{
    Widget widget;
    Canvas_Script canvas;
    public Object_attributes attachedObject;
    [HideInInspector] public Children currentChild_atTarget;
    [HideInInspector] public Animation_Script attachedObject_Animation;


    public int animation_Index;
    public float waitTime_seconds = 2.0f;   
    float timer;
    [HideInInspector] public bool isOpen; 
    [HideInInspector] public bool isTargeted;
    [HideInInspector] public bool isWaitTarget;
    public bool isDeadly;
    public bool isConsumable;
    bool isTaped;
    bool timerDown;


    string canvas_name = "InGameUI";
    


    private void Start() {
        canvas = GameObject.Find(canvas_name).GetComponent<Canvas_Script>();
        if(attachedObject != null && attachedObject.GetComponent<Animation_Script>() != null) attachedObject_Animation = attachedObject.GetComponent<Animation_Script>();
        if(attachedObject == null && !isConsumable) isWaitTarget = true;
        if(attachedObject_Animation != null) attachedObject_Animation.SetAnimationSpeed(waitTime_seconds);

        timer = 1.0f;
        timerDown = true;
    }

    public void Animate_AttachedObject()
    {
        if(!isTaped && attachedObject_Animation != null) attachedObject_Animation.PlayAnimation(animation_Index, true, true);
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
                    if(attachedObject_Animation != null) attachedObject_Animation.PlayAnimation(animation_Index, false, true);
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
                child.animation_Script.PlayAnimation(animation_Index, false, false);
                if(attachedObject_Animation != null) attachedObject_Animation.PlayAnimation(animation_Index, false, true);
                Destroy(widget.gameObject);
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

    public void InstantiateWidget_target(Vector3 widget_worldPos, Color color)
    {
        widget = canvas.InstantiateWidget(widget_worldPos, color);
        ResetTimer();
    }


    public void DestroyWidget()
    {
        if(widget != null) Destroy(widget.gameObject);
    }

    void ResetTimer()
    {
        timer = 1.0f;
    }

    public void SetCurrentChild(Children child)
    {
        currentChild_atTarget = child;
    }
}
