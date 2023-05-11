using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Children : MonoBehaviour
{

    Canvas_Script canvas;
    public Target currentTarget;
    Target tempOldTarget;
    NavMeshAgent attachedAgent; 
    NavMesh navMesh;
    Widget widget;
    float waitTime_seconds; 
    float timer, stuckTimer;
    bool timerUp, timerDown;
    bool startTimer, startStuckTimer;
    bool isInSafeZone;
    public bool isStopped;

    private void Start() 
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas_Script>();
 
        attachedAgent = this.GetComponent<NavMeshAgent>();
        currentTarget = null;
        navMesh = GameObject.Find("NavMesh_Handler").GetComponent<NavMesh>();
    }

    void Update()
    {
        if(startTimer) Timer();
        if(startStuckTimer) StuckTimer();
    }


    public void SetTarget(Target t)
    {
        currentTarget = t;
        waitTime_seconds = currentTarget.waitTime_seconds;
        ResetTimer();
        //Debug.Log(currentTarget.name + " set as currentTarget");
    }

    public Target GetTarget() 
    { 
        return currentTarget; 
    }

    private void OnTriggerEnter(Collider other)
    {
        startStuckTimer = true;
        GameObject triggerObject = other.gameObject;
        
        //normal target

            if (triggerObject.GetComponent<Target>() == currentTarget)
            {
                Vector3 widget_pos = other.gameObject.transform.GetChild(0).gameObject.transform.position;
                Color color;
                
                if(currentTarget.isDeadly) color = Color.red;
                else color = Color.green;

                widget = canvas.InstantiateWidget(widget_pos, waitTime_seconds, color);

                currentTarget.isOpen = false;
                startTimer = true;
                timerDown = true;

                if(!currentTarget.isDeadly) isInSafeZone = true;
            }


        //consumable
        else if(other.tag == "consumable_radius" && !isInSafeZone)
        {
            Target consumableTarget = triggerObject.transform.GetComponentInParent<Target>();
            if(consumableTarget.isOpen)
            {   
                Consumables consumable = triggerObject.GetComponentInParent<Consumables>();
                if(widget != null) Destroy(widget.gameObject);

                SetTarget(consumableTarget);
                navMesh.SetSpecificPath(attachedAgent, consumableTarget);
                consumable.StartExistenceTimer(false);

                consumableTarget.isOpen = false;
            }            
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        Debug.Log(other.name);
        if(stuckTimer >= waitTime_seconds + 3)
        {
            Reset(); //checks, if child is stuck at a target
            startStuckTimer = false;
        } 
    }

    private void OnTriggerExit(Collider other) 
    {
        IsSafeZone(false);
    }


    void Timer()
    {
        //timer goes down
        if(timerDown)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                //Debug.Log("Timer() - is Zero");
                if(currentTarget.isDeadly)
                {
                    currentTarget.isOpen = true;
                    currentTarget.isTargeted = false;
                    navMesh.Remove_Agent(this.GetComponent<NavMeshAgent>());
                    Destroy(gameObject); 
                }

                else 
                    Reset();

                Destroy(widget.gameObject);
            }
        }

        //timer goes up (until waitTime is reached)
        else if(timerUp)
        {
            if (timer >= waitTime_seconds) ResetTimer();
            timer += Time.deltaTime / waitTime_seconds;
        }

        widget.UpdateWidget(timer);
    }

    void StuckTimer()
    {
        stuckTimer += Time.deltaTime;
    }

    void IsSafeZone(bool isActivated)
    {
        if (isActivated) isInSafeZone = true;
        else isInSafeZone = false;
    }

    void Reset()
    {
        
        if(currentTarget.isConsumable)
        {
            Debug.Log(currentTarget.name);
            Destroy(currentTarget.gameObject);
            navMesh.RecalculatePath(attachedAgent);
        }
        
        else{
            tempOldTarget = currentTarget;
            navMesh.RecalculatePath(attachedAgent);

            if(!attachedAgent.isStopped)
            {
                navMesh.Un_target(tempOldTarget);
            }
        }
        
        IsSafeZone(false);

        ResetTimer();
    }

    void ResetTimer()
    {
        timer = waitTime_seconds;
        stuckTimer = 0.0f;
        startTimer = false;
        timerDown = false;
    }
}
