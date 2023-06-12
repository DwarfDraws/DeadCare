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
    [SerializeField] GameObject btn_move, btn_tape;
    [SerializeField] GameObject pnl_GameOver;
    [SerializeField] TMP_Text txt_YouWin, txt_ChildrenCounter;
    [SerializeField] TMP_Text txt_Countdown;
    [SerializeField] TMP_Text txt_tapeCounter;
    bool prepCountDownStart, gameCountdownStart;
    bool spawnChildAfterCountdown;
    float init_prepCountdownTimer, current_prepCountdownTimer;
    float init_gameCountdownTimer, current_gameCountdownTimer;
    int displayTimer;
    bool isGameOver;

    private void Start() 
    {
        obstacles = GameObject.FindGameObjectsWithTag("obstacle");
    }  

    private void Update() 
    {
        if(prepCountDownStart)
        {
            current_prepCountdownTimer -= Time.deltaTime;
            displayTimer = (int)current_prepCountdownTimer + 1;
            txt_Countdown.text = displayTimer.ToString();
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
            txt_Countdown.text = displayTimer.ToString();
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
        btn_move.SetActive(false);
        btn_tape.SetActive(false);
        
        canvas.Deactivate_MoveButton();
        canvas.Deactivate_TapeButton();
        txt_Countdown.text = "movement locked";
        canvas.SetMoveableHalosActive(false);

        spawner.SpawnChildren();

        gameCountdownStart = true;
    }

    public void GameOver(bool isWon)
    {
        if(!isGameOver)
        {
            gameCountdownStart = false;
            current_gameCountdownTimer = init_gameCountdownTimer;
            txt_Countdown.text = "--";

            //GameOver-Panel
            int survivedChildren = gameplay.GetChildCount();
            int initial_childAmount = gameplay.init_childCounter;
            txt_ChildrenCounter.text = survivedChildren.ToString() + "/" + initial_childAmount.ToString() + " children survived";
            if(isWon) txt_YouWin.text = "You win!";
            else txt_YouWin.text = "You lose!";
            pnl_GameOver.SetActive(true);

            isGameOver = true;
            
            foreach(GameObject child in GameObject.FindGameObjectsWithTag("child")) 
            {
                child.GetComponent<Children>().ChildDestroy();
            }
            foreach(GameObject obstacle in GameObject.FindGameObjectsWithTag("obstacle"))
            {
                Object_attributes oa = obstacle.GetComponent<Object_attributes>();
                oa.Animate(1);
                oa.SetTapeActive(false);
                
            }
            foreach(GameObject widget in GameObject.FindGameObjectsWithTag("widget")){
                Destroy(widget);
            }
        }
    }

    public void UpdateTapeCounter(int count){
        txt_tapeCounter.text = count.ToString();
    }

}
