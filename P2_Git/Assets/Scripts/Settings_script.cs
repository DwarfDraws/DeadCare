using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings_script : MonoBehaviour
{
    [SerializeField] Menu_Handler menu_Handler;


    public bool consumablesHaveExistenceTimer;
    public bool isTutorial;
    [SerializeField] float prepCountdownTime_Seconds;
    [SerializeField] float gameCountdownTime_Seconds;
    [SerializeField] int children_Amount = 0;

    private void Start() 
    {
        menu_Handler.SetPrepCountdownTimer(prepCountdownTime_Seconds);
        menu_Handler.SetGameCountdownTimer(gameCountdownTime_Seconds);
        menu_Handler.SpawnChildrenAfterCountdown(children_Amount);
    }

}
