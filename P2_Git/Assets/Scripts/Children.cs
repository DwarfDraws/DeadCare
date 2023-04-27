using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Children : MonoBehaviour
{

    Target target;
    [SerializeField] NavMesh navMesh;
    float waitTime; //seconds
    float timer;
    bool timerUp, timerDown;
    bool startTimer;


    void Update()
    {
         if(startTimer) Timer();
    }

    private void OnTriggerEnter(Collider other)
    {
        target = other.gameObject.GetComponent<Target>();
        
        waitTime = target.waitTime_seconds;
        ResetTimer();

        startTimer = true;
        timerDown = true;
    }

    //reset Values when leaving
    private void OnTriggerExit(Collider other) {

        ResetTimer();
        navMesh.ReOpenTarget(other);
    }


    void Timer(){
        
        //timer goes down
        if(timerDown)
        {
            timer -= Time.deltaTime;
           
            if(timer <= 0){
                //Debug.Log("Timer() - is Zero");
                if(target.isDeadly) Destroy(gameObject); 
                else{ 
                    navMesh.isPathRecalculated = false;
                    startTimer = false;
                    timerDown = false; 
                }
            }
        }

        //timer goes up until waitTime is reached again
        else if(timerUp && timer < waitTime)
        {
            timer += Time.deltaTime / waitTime;
        }
    }

    void ResetTimer(){
        timer = waitTime;
    }
}
