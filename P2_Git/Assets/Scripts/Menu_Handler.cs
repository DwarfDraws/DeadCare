using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Menu_Handler : MonoBehaviour
{
    SaveData saveData;
    
    [SerializeField] Raycast raycast;
    [SerializeField] Spawner spawner;
    [SerializeField] Canvas_Script canvas;
    [SerializeField] Gameplay gameplay;
    [SerializeField] NavMesh navMesh;

    [SerializeField] UnityEvent PrepPhaseStart;
    [SerializeField] UnityEvent NextPhaseStart;
    [SerializeField] UnityEvent GameWon;

    public List<GameObject> ingame_Consumables = new List<GameObject>();
    [SerializeField] GameObject[] reward_stars;
    [SerializeField] GameObject noMoveArea_Spawn;
    GameObject[] obstacles;

    [HideInInspector] public int countdownTxt_Size_MAX;
    [HideInInspector] public int countdownTxt_TimeFromWhenScale;
    int countdown_Speed;
    int displayTimer, tmp_displayTimer;
    float countdownTxt_Size_init;
    float countdownTxt_Size;
    float init_prepCountdownTimer, current_prepCountdownTimer;
    float init_gameCountdownTimer, current_gameCountdownTimer;
    [HideInInspector] public bool isGameOver;
    public bool isTutorial;
    bool isCountdown_ScaleUP;
    bool isCountdownSpedUp;
    bool prepCountDownStart, gameCountdownStart;
    bool spawnChildAfterCountdown;

    string door_name = "pref_door";
    string tag_obstacle = "obstacle";
    string tag_child = "child";
    string tag_widget = "widget";

    private void Start() 
    {
        countdown_Speed = 1;
        if(canvas != null) countdownTxt_Size_init = canvas.GetCountdown_Txt_Size();
        countdownTxt_Size = countdownTxt_Size_init;

        obstacles = GameObject.FindGameObjectsWithTag(tag_obstacle);
    }  

    private void Update() 
    {
        //prep-Countdown
        if(prepCountDownStart)
        {
            current_prepCountdownTimer -= Time.deltaTime * countdown_Speed;

            if(current_prepCountdownTimer < 0)
            {
                prepCountDownStart = false;
                current_prepCountdownTimer = init_prepCountdownTimer;

                NextPhase();
                NextPhaseStart?.Invoke();
                return;
            } 

            displayTimer = (int)current_prepCountdownTimer + 1;
            

            //Sizing Countdown_Txt
            if(tmp_displayTimer != displayTimer && displayTimer <= countdownTxt_TimeFromWhenScale) 
            {
                countdownTxt_Size = countdownTxt_Size_MAX - 20; //making countdownTxt_Size an int-value
                isCountdown_ScaleUP = true;
            }
            if(isCountdown_ScaleUP && countdownTxt_Size != countdownTxt_Size_MAX) 
            {
                countdownTxt_Size += 2;
                if(countdownTxt_Size == countdownTxt_Size_MAX) isCountdown_ScaleUP = false;
            }
            else if (!isCountdown_ScaleUP && countdownTxt_Size != countdownTxt_Size_init) countdownTxt_Size--;

            tmp_displayTimer = displayTimer;


            canvas.SetCountdown_Txt(displayTimer.ToString(), countdownTxt_Size);
        } 
        
        //gameplay-Countdown
        if(gameCountdownStart)
        {
            current_gameCountdownTimer -= Time.deltaTime;
            displayTimer = (int)current_gameCountdownTimer + 1;
            canvas.SetCountdown_Txt(displayTimer.ToString(), countdownTxt_Size_init);
        }
        if(current_gameCountdownTimer < 0)
        {
            GameOver(true);
        }
    }




    public void setCountdown_Speed(int speed)
    {
        countdown_Speed = speed;
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

    public void StartCountdown()
    {
        PrepPhaseStart?.Invoke();
        isGameOver = false; //in case of restart
        prepCountDownStart = true;
        countdown_Speed = 1;

        noMoveArea_Spawn.SetActive(true);

        canvas.ActivateButton_Consumable(false); //Cookies are not available in Prep-Phase!
        if (!isTutorial) canvas.btn_skipCountdown.SetActive(true);
    }




    void NextPhase()
    {
        if(!isTutorial) canvas.ActivateButton_Consumable(true);
        canvas.btn_move.SetActive(false);
        canvas.btn_tape.SetActive(false);
        canvas.btn_skipCountdown.SetActive(false); 
        canvas.Deactivate_MoveButton();
        canvas.Deactivate_TapeButton();
        canvas.SetMoveableHalosActive(false);
        canvas.SetTapeableHalosActive(false);

        noMoveArea_Spawn.SetActive(false);

        Animator door_anim = GameObject.Find(door_name).GetComponent<Animator>();
        door_anim.SetTrigger("türAuf");
        spawner.SpawnChildren();

        gameCountdownStart = true;
    }

    public void GameOver(bool isWon)
    {
        if (isGameOver) return;

        GameWon?.Invoke();

        isGameOver = true;
        gameCountdownStart = false;
        current_gameCountdownTimer = init_gameCountdownTimer;
        canvas.SetCountdown_Txt(" ", countdownTxt_Size_init);
        canvas.btn_consumable.SetActive(false);
        canvas.SetWidgetsActive(false);

        //GameOver-Panel
        int survivedChildren = gameplay.GetChildCount();
        int initial_childAmount = gameplay.init_childCounter;
        int starReward_Count = CalculateStarReward(survivedChildren, initial_childAmount);
        canvas.SetChildrenCounter_Txt(survivedChildren.ToString() + "/" + initial_childAmount.ToString());
        AddScoreReward(starReward_Count);
        canvas.SetStarImages(starReward_Count);
        if(survivedChildren <= 1) canvas.btn_NextLevel.SetActive(false);
        else canvas.btn_NextLevel.SetActive(true);
        if(!isTutorial) canvas.pnl_GameOver.SetActive(true);

        foreach(GameObject star in reward_stars)
        {
            star.GetComponent<DOTweenScale>().StartTweening();
        }


        //Reset Children    
        foreach(GameObject child in GameObject.FindGameObjectsWithTag(tag_child)) 
        {
            child.GetComponent<Children>().ChildDestroy();
        }

        //Reset Obstacles
        int i = 0;
        foreach(GameObject obstacle in GameObject.FindGameObjectsWithTag(tag_obstacle))
        {
            Object_attributes oa = obstacle.GetComponent<Object_attributes>();
            oa.SetTapeActive(false);

            if(obstacle.GetComponentInChildren<Animation_Script>() != null)
            {
                Animation_Script anim_script = obstacle.GetComponentInChildren<Animation_Script>();
                    
                anim_script.PlayAnimation(oa.attachedTarget.animation_Index, false, true, false);
                anim_script.PlayAnimation(oa.attachedTarget.animation_Index, false, true, true);
            }
            i++;
        }

        //Reset Consumables
        foreach(GameObject consumable in ingame_Consumables)
        {
            Destroy(consumable);
        }

        //Reset Widgets
        foreach(GameObject widget in GameObject.FindGameObjectsWithTag(tag_widget))
        {
            Destroy(widget);
        }
            
    }


    void AddScoreReward(int newScore)
    {
        saveData = SaveManager.Load();
        saveData.score += newScore;
        SaveManager.Save(saveData);
    }

    public void DecreaseScore()
    {
        saveData = SaveManager.Load();
        saveData.score--;
        SaveManager.Save(saveData);
    }

    public void ResetScore()
    {
        saveData = SaveManager.Load();
        saveData.score = 0;
        SaveManager.Save(saveData);
    }

    public int GetScore()
    {
        saveData = SaveManager.Load();
        return saveData.score;
    } 


    int CalculateStarReward(int survivedChildren, int init_childCounter)
    {
        if(survivedChildren == init_childCounter) return 3;
        else if(survivedChildren / (float)init_childCounter > 0.5f) return 2;
        else if(survivedChildren >= 1) return 1;
        else return 0;
    }
}
