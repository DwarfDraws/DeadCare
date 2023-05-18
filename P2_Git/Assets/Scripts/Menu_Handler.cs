using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu_Handler : MonoBehaviour
{
    [SerializeField] Raycast raycast;
    [SerializeField] Spawner spawner;

    GameObject[] obstacles;
    [SerializeField] TMP_Text txt_Countdown;
    bool prepCountDownStart, gameCountdownStart;
    bool spawnChildAfterCountdown;
    float prepCountdownTimer, gameCountdownTimer;
    int childrenAmount;
    int displayTimer;

    private void Start() 
    {
        obstacles = GameObject.FindGameObjectsWithTag("obstacle");
    }  

    private void Update() 
    {
        if(prepCountDownStart)
        {
            prepCountdownTimer -= Time.deltaTime;
            displayTimer = (int)prepCountdownTimer + 1;
            txt_Countdown.text = displayTimer.ToString();
        } 
        if(prepCountdownTimer < 0)
        {
            prepCountDownStart = false;
            prepCountdownTimer = 255;

            NextPhase();
        } 

        if(gameCountdownStart)
        {
            gameCountdownTimer -= Time.deltaTime;
            displayTimer = (int)gameCountdownTimer + 1;
            txt_Countdown.text = displayTimer.ToString();
        }
        if(gameCountdownTimer < 0)
        {
            gameCountdownStart = false;
            gameCountdownTimer = 255;

            txt_Countdown.text = "--";
        }
    }

    public void SetPrepCountdownTimer(float f)
    {
        prepCountdownTimer = f;
    }
    public void SetGameCountdownTimer(float f)
    {
        gameCountdownTimer = f;
    }

    public void SpawnChildrenAfterCountdown(int amount)
    {
        childrenAmount = amount;
    }

    public void StartCountdown(){
        prepCountDownStart = true;
    }
    
    void MakeObjectsUnmoveable()
    {    
        foreach(GameObject oa in obstacles)
        {
            oa.GetComponent<Object_attributes>().isMoveable = false;
        }

        raycast.isMoveable = false;
    }


    void NextPhase()
    {
        MakeObjectsUnmoveable();
        txt_Countdown.text = "movement locked";
        if(childrenAmount > 0) spawner.SpawnChild();

        gameCountdownStart = true;
    }

}
