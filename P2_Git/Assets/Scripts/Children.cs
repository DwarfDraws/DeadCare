using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Children : MonoBehaviour
{

    //[SerializeField] Canvas_Script canvas;
    Target_attributes currentTarget; //used to check for trigger-events
    Target_attributes tempOldTarget;
    NavMeshAgent attachedAgent; 
    NavMesh navMesh;
    float waitTime; //seconds
    float timer, stuckTimer;
    bool timerUp, timerDown;
    bool startTimer;

    private void Start() {
        attachedAgent = this.GetComponent<NavMeshAgent>();
        currentTarget = null;
        navMesh = GameObject.Find("NavMesh_Handler").GetComponent<NavMesh>();
    }

    void Update()
    {
         if(startTimer){
            Timer();
            StuckTimer();
         }
    }


    public void SetTarget(Target_attributes t)
    {
        currentTarget = t;
        waitTime = currentTarget.waitTime_seconds;
        ResetTimer();
        //Debug.Log(currentTarget.name + " set as currentTarget");
    }
    public Target_attributes GetTarget() { return currentTarget; }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.GetComponent<Target_attributes>().name);
        if (other.gameObject.GetComponent<Target_attributes>() == currentTarget){
            currentTarget.isOpen = false;
            startTimer = true;
            timerDown = true;
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(stuckTimer >= waitTime + 1) Reset(); //checks, if child is stuck at a target
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.GetComponent<Target_attributes>() == currentTarget){
            currentTarget.isOpen = true;
        }
    }


    void Timer()
    {
        //timer goes down
        if(timerDown)
        {
            timer -= Time.deltaTime;
            if(currentTarget.isDeadly) Debug.Log(timer);

            if(timer <= 0){
                //Debug.Log("Timer() - is Zero");
                if(currentTarget.isDeadly){
                    navMesh.RemoveAgent(this.GetComponent<NavMeshAgent>());
                    Destroy(gameObject); 
                }
                else Reset();
            }
        }

        //timer goes up (until waitTime is reached)
        else if(timerUp){
            if (timer >= waitTime) ResetTimer();
            timer += Time.deltaTime / waitTime;
        }

        //canvas.UpdateSlider(timer);
    }

    void StuckTimer(){
        stuckTimer += Time.deltaTime;
    }

    void Reset(){
        tempOldTarget = currentTarget;
        navMesh.RecalculatePath(attachedAgent);           
        navMesh.Un_target(tempOldTarget);
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
