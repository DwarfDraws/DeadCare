using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] Menu_Handler menu_Handler;
    [SerializeField] Spawner spawner;
    
    public int init_tapeCounter;
    int current_tapeCounter;
    public int init_childCounter;
    int current_ChildCounter;

    
    private void Start() {       
        current_ChildCounter = init_childCounter;
        current_tapeCounter = init_tapeCounter;
    }


    public void DecreaseChildCount()
    {
        current_ChildCounter--;
        if(current_ChildCounter == 0) menu_Handler.GameOver(false);
    }
    public int GetChildCount()
    {
        return current_ChildCounter;
    }
    public void ResetChildCount()
    {
        current_ChildCounter = init_childCounter;
        spawner.childrenToSpawn = init_childCounter;
    }


    public void DecreaseTapeCount() 
    {
        current_tapeCounter--;
        menu_Handler.UpdateTapeCounter(current_tapeCounter);
    }
    public void IncreaseTapeCount() 
    {
        current_tapeCounter++;
        menu_Handler.UpdateTapeCounter(current_tapeCounter);
    }
    public void ResetTapeCount()
    {
        current_tapeCounter = init_tapeCounter;
        menu_Handler.UpdateTapeCounter(current_tapeCounter);
    }
    public int GetTapeCount()
    {
        return current_tapeCounter;
    }
}
