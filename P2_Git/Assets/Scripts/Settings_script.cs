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

    public bool consumablesHaveExistenceTimer;
    public bool isTutorial;
    [SerializeField] bool hideTargets;
    [SerializeField] float prepCountdownTime_Seconds;
    [SerializeField] float gameCountdownTime_Seconds;
    [SerializeField] int children_Amount = 0;
    [SerializeField] int tape_Amount = 0;

    private void Awake() 
    {
        menu_Handler.SetPrepCountdownTimer(prepCountdownTime_Seconds);
        menu_Handler.SetGameCountdownTimer(gameCountdownTime_Seconds);
        spawner.childrenToSpawn = this.children_Amount;
        gameplay.init_childCounter = this.children_Amount;
        gameplay.init_tapeCounter = tape_Amount;
        canvas.SetTapeCounter_Txt(tape_Amount.ToString());


        if (isTutorial) btnSpawnChildren.gameObject.SetActive(false);
        if (hideTargets) HideTargets();
    }



    void HideTargets()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("target");
        foreach(GameObject target in targets)
        {
            target.GetComponent<MeshRenderer>().enabled = false;
        }
    }

}
