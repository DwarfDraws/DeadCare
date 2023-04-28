using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Children : MonoBehaviour
{

    Canvas_Script canvas;
    Target currentTarget; //used to check for trigger-events
    Target tempOldTarget;
    NavMeshAgent attachedAgent; 
    NavMesh navMesh;
    Widget widget;
    float waitTime; //seconds
    float timer, stuckTimer;
    bool timerUp, timerDown;
    bool startTimer, startStuckTimer;
    public bool isStopped;

    private void Start() {
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
        waitTime = currentTarget.waitTime_seconds;
        ResetTimer();
        //Debug.Log(currentTarget.name + " set as currentTarget");
    }

    public Target GetTarget() 
    { 
        return currentTarget; 
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.GetComponent<Target_attributes>().name);
        if (other.gameObject.GetComponent<Target>() == currentTarget){
            
            Vector3 widget_pos = other.gameObject.transform.GetChild(0).gameObject.transform.position;
            widget = canvas.InstantiateWidget(widget_pos, waitTime, currentTarget.isDeadly);

            currentTarget.isOpen = false;
            startTimer = true;
            timerDown = true;
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(stuckTimer >= waitTime + 3){
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
                    navMesh.RemoveAgent(this.GetComponent<NavMeshAgent>());
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
            if (timer >= waitTime) ResetTimer();
            timer += Time.deltaTime / waitTime;
        }

        widget.UpdateTimer(timer);
    }

    void StuckTimer()
    {
        stuckTimer += Time.deltaTime;
    }

    void Reset()
    {
        
        tempOldTarget = currentTarget;
        navMesh.RecalculatePath(attachedAgent);           
        if(!attachedAgent.isStopped) navMesh.Un_target(tempOldTarget);
        ResetTimer();
    }

    void ResetTimer()
    {
        timer = waitTime;
        stuckTimer = 0.0f;
        startTimer = false;
        timerDown = false;
    }
}
