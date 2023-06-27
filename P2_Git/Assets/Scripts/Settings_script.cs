using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings_script : MonoBehaviour
{
    [SerializeField] Menu_Handler menu_Handler;
    [SerializeField] Raycast raycast;
    [SerializeField] Spawner spawner;
    [SerializeField] Gameplay gameplay;
    [SerializeField] Button btnSpawnChildren;
    [SerializeField] Canvas_Script canvas;
    [SerializeField] ChildCounter childCounter;
    [SerializeField] DOTweenScale star_DOTweenScale;

    //Inspector tweak variables

    [Header("Gameplay")]
    [SerializeField] int children_Amount = 0;
    [SerializeField] int tape_Amount = 0;
    [SerializeField] int consumable_Amount = 0;
    [SerializeField] float children_walkSpeed;

    [Header("Countdowns")]
    [SerializeField] float prepTime_Seconds;
    [SerializeField] float gameTime_Seconds;
    [SerializeField] int countdownTxt_Size_MAX;
    [SerializeField] int timeFromWhenToScale;
    
    [Header("Tutorial")]
    public bool isTutorial;
    [SerializeField] Children tutorial_Child;
    [SerializeField] List<Target> tutorialTargets;

    [Header("Stars_Animation")]
    [SerializeField] float stars_tweeningFactor;
    [SerializeField] float stars_tweeningDuration_seconds;
    [SerializeField] float stars_tweeningOffset_seconds;
    [SerializeField] float waitTime_atMax_seconds;

    [Header("Miscellaneous")]
    [SerializeField] bool hideTargets;
    public bool consumablesHaveExistenceTimer;
    [SerializeField] bool creatorMode;


    string tag_target = "target";

    private void Awake() 
    {
        menu_Handler.SetPrepCountdownTimer(prepTime_Seconds);
        menu_Handler.SetGameCountdownTimer(gameTime_Seconds);
        menu_Handler.countdownTxt_Size_MAX = this.countdownTxt_Size_MAX;
        menu_Handler.countdownTxt_TimeFromWhenScale = timeFromWhenToScale;
        
        spawner.childrenToSpawn = this.children_Amount;
        spawner.children_walkSpeed = this.children_walkSpeed;

        gameplay.SetChildrenCount(children_Amount);
        gameplay.SetTapeCount(tape_Amount);

        if(creatorMode)
        {
            gameplay.isCreatorMode = true;
            gameplay.SetConsumableCount(consumable_Amount);
        } 
        else
        {
            canvas.btn_spawnChild.SetActive(false);
            canvas.btn_start.SetActive(false);
            menu_Handler.StartCountdown();
        }

        childCounter.SetChildrenAmount(children_Amount);

        if (isTutorial)
        {
            btnSpawnChildren.gameObject.SetActive(false);
            if (tutorial_Child != null && tutorialTargets != null) tutorial_Child.tutorialTargets = tutorialTargets;
            else if(tutorial_Child == null) Debug.Log("Settings_Script(): missing tutorial-child!");
            else if(tutorialTargets == null) Debug.Log("Settings_Script(): missing tutorial-targets!");
        }
        if (hideTargets) HideTargets();

        star_DOTweenScale.SetupTweening(stars_tweeningFactor, stars_tweeningDuration_seconds, stars_tweeningOffset_seconds, waitTime_atMax_seconds);
    }



    void HideTargets()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag_target);
        foreach(GameObject target in targets)
        {
            target.GetComponent<MeshRenderer>().enabled = false;
        }
    }

}
