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

    [SerializeField] int children_Amount = 0;
    [SerializeField] int tape_Amount = 0;
    [SerializeField] int consumable_Amount = 0;
    [SerializeField] float prepCountdownTime_Seconds;
    [SerializeField] float gameCountdownTime_Seconds;
    [SerializeField] bool hideTargets;
    public bool consumablesHaveExistenceTimer;
    public bool isTutorial;


    string tag_target = "target";

    private void Awake() 
    {
        menu_Handler.SetPrepCountdownTimer(prepCountdownTime_Seconds);
        menu_Handler.SetGameCountdownTimer(gameCountdownTime_Seconds);
        spawner.childrenToSpawn = this.children_Amount;
        gameplay.SetChildrenCount(children_Amount);
        gameplay.SetTapeCount(tape_Amount);
        gameplay.SetConsumableCount(consumable_Amount);

        if (isTutorial) btnSpawnChildren.gameObject.SetActive(false);
        if (hideTargets) HideTargets();
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
