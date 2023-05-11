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
        GameObject triggerObject = other.gameObject;
        
        if(triggerObject.GetComponent<Target>() != null){
            if (triggerObject.GetComponent<Target>() == currentTarget){
            
            Vector3 widget_pos = other.gameObject.transform.GetChild(0).gameObject.transform.position;
            widget = canvas.InstantiateWidget(widget_pos, waitTime_seconds, currentTarget.isDeadly);

            currentTarget.isOpen = false;
            startTimer = true;
            timerDown = true;
            }
        }

        else if(other.tag == "consumable_radius")
        {
            Target consumableTarget = triggerObject.transform.GetComponentInParent<Target>();
            navMesh.SetSpecificPath(attachedAgent, consumableTarget);
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(stuckTimer >= waitTime_seconds + 3){
            Debug.Log("Was stuck");
            Reset(); //checks, if child is stuck at a target
            startStuckTimer = false;
        } 
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.GetComponent<Target>() == currentTarget){
            currentTarget.isOpen = true;
        }
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

        widget.UpdateTimer(timer);
    }

    void StuckTimer()
    {
        stuckTimer += Time.deltaTime;
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
