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
    public Animation_Script attachedObject_Animation;


    public int animation_Index;
    public float waitTime_seconds = 2.0f;   
    float timer;
    float timeTillDeath_seconds;
    [HideInInspector] public bool isOpen; 
    [HideInInspector] public bool isTargeted;
    [HideInInspector] public bool isWaitTarget;
    public bool isDeadly;
    public bool isConsumable;
    bool isTaped;
    bool timerDown;
    bool childDies;


    string canvas_name = "InGameUI";
    


    private void Start() 
    {
        canvas = GameObject.Find(canvas_name).GetComponent<Canvas_Script>();
        if(attachedObject != null && attachedObject.GetComponent<Animation_Script>() != null) attachedObject_Animation = attachedObject.GetComponent<Animation_Script>();
        if(attachedObject == null && !isConsumable) isWaitTarget = true;

        if(attachedObject_Animation != null) attachedObject_Animation.SetAnimationSpeed(animation_Index, waitTime_seconds, true);

        timer = 1.0f;
        timerDown = true;
    }

    private void Update() 
    {
        //death-countdown bc of death-animation-timer
        if(childDies)
        {
            timeTillDeath_seconds -= Time.deltaTime;

            if(timeTillDeath_seconds <= 0)
            { 
                Animation_Script o_anim = attachedObject_Animation;
                Animation_Script c_anim = currentChild_atTarget.animation_script;

                //idle-anims
                o_anim.PlayAnimation(animation_Index, false, true, false);
                c_anim.PlayAnimation(animation_Index, false, false, false);
                
                //death-anims
                o_anim.PlayAnimation(animation_Index, false, true, true);
                c_anim.PlayAnimation(animation_Index, false, false, true);

                currentChild_atTarget.ChildDestroy();
                childDies = false;
            }
        }    
    }

    public void Animate_AttachedObject()
    {
        if(!isTaped && attachedObject_Animation != null) attachedObject_Animation.PlayAnimation(animation_Index, true, true, false);
    }

    public void Timer()
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
                    currentChild_atTarget.Reset(); 
                    ResetTimer();
                }
                
                else if(isDeadly)
                {
                    isOpen = true;
                    isTargeted = false;
                    
                    if(attachedObject_Animation != null)
                    {
                        Animation_Script o_anim = attachedObject_Animation;
                        Animation_Script c_anim = currentChild_atTarget.animation_script;                       
                        
                        timeTillDeath_seconds = attachedObject_Animation.Get_DeathAnimClipLength(animation_Index, true);
                        o_anim.PlayAnimation(animation_Index, true, true, true);
                        c_anim.PlayAnimation(animation_Index, true, false, true);

                    } 
                    
                    childDies = true;
                    currentChild_atTarget.ResetTimer();
                    ResetTimer();
                }

                else 
                {
                    currentChild_atTarget.Reset();
                    ResetTimer();
                }

                DestroyWidget();
            }
        }

        //timer goes up
        else if(!timerDown)
        {
            timer += Time.deltaTime / waitTime_seconds;
            //Debug.Log(timer);

            if (timer > 1.0f)
            { 
                currentChild_atTarget.Reset();
                currentChild_atTarget.animation_script.PlayAnimation(animation_Index, false, false, false);
                if(attachedObject_Animation != null) attachedObject_Animation.PlayAnimation(animation_Index, false, true, false);
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
