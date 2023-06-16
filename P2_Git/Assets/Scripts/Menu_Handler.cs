using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu_Handler : MonoBehaviour
{
    [SerializeField] Raycast raycast;
    [SerializeField] Spawner spawner;
    [SerializeField] Canvas_Script canvas;
    [SerializeField] Gameplay gameplay;
    [SerializeField] NavMesh navMesh;

    GameObject[] obstacles;

    int displayTimer;
    float init_prepCountdownTimer, current_prepCountdownTimer;
    float init_gameCountdownTimer, current_gameCountdownTimer;
    [HideInInspector] public bool isGameOver;
    bool prepCountDownStart, gameCountdownStart;
    bool spawnChildAfterCountdown;

    string tag_obstacle = "obstacle";
    string tag_child = "child";
    string tag_widget = "widget";

    private void Start() 
    {
        obstacles = GameObject.FindGameObjectsWithTag(tag_obstacle);
    }  

    private void Update() 
    {
        if(prepCountDownStart)
        {
            current_prepCountdownTimer -= Time.deltaTime;
            displayTimer = (int)current_prepCountdownTimer + 1;
            canvas.SetCountdown_Txt(displayTimer.ToString());
        } 
        if(current_prepCountdownTimer < 0)
        {
            prepCountDownStart = false;
            current_prepCountdownTimer = init_prepCountdownTimer;

            NextPhase();
        } 

        if(gameCountdownStart)
        {
            current_gameCountdownTimer -= Time.deltaTime;
            displayTimer = (int)current_gameCountdownTimer + 1;
            canvas.SetCountdown_Txt(displayTimer.ToString());
        }
        if(current_gameCountdownTimer < 0)
        {
            GameOver(true);
        }
    }

    public void SetPrepCountdownTimer(float f)
    {
        init_prepCountdownTimer = f;
        current_prepCountdownTimer = init_prepCountdownTimer;
    }
    public void SetGameCountdownTimer(float f)
    {
        init_gameCountdownTimer = f;
        current_gameCountdownTimer = init_gameCountdownTimer;
    }

    public void StartCountdown(){
        isGameOver = false; //in case of restart
        prepCountDownStart = true;
    }

    void NextPhase()
    {
        canvas.btn_move.SetActive(false);
        canvas.btn_tape.SetActive(false);   
        canvas.Deactivate_MoveButton();
        canvas.Deactivate_TapeButton();
        canvas.SetMoveableHalosActive(false);
        canvas.SetTapeableHalosActive(false);

        spawner.SpawnChildren();

        gameCountdownStart = true;
    }

    public void GameOver(bool isWon)
    {
        if(!isGameOver)
        {
            gameCountdownStart = false;
            current_gameCountdownTimer = init_gameCountdownTimer;
            canvas.SetCountdown_Txt(" ");

            //GameOver-Panel
            int survivedChildren = gameplay.GetChildCount();
            int initial_childAmount = gameplay.init_childCounter;
            canvas.SetChildrenCounter_Txt(survivedChildren.ToString() + "/" + initial_childAmount.ToString());
            if(isWon) canvas.SetYouWin(true);
            else canvas.SetYouWin(false);
            canvas.pnl_GameOver.SetActive(true);

            isGameOver = true;
            
            foreach(GameObject child in GameObject.FindGameObjectsWithTag(tag_child)) 
            {
                child.GetComponent<Children>().ChildDestroy();
            }

            int i = 0;
            foreach(GameObject obstacle in GameObject.FindGameObjectsWithTag(tag_obstacle))
            {
                Object_attributes oa = obstacle.GetComponent<Object_attributes>();
                oa.SetTapeActive(false);

                if(obstacle.GetComponent<Animation_Script>() != null)
                {
                    obstacle.GetComponent<Animation_Script>().PlayAnimation(oa.attachedTarget.animation_Index, false, true, false);
                }
                i++;
            }
            foreach(GameObject widget in GameObject.FindGameObjectsWithTag(tag_widget)){
                Destroy(widget);
            }
        }
    }
}
