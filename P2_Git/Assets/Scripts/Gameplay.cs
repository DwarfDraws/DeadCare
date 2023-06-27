using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] Menu_Handler menu_Handler;
    [SerializeField] Spawner spawner;
    [SerializeField] Canvas_Script canvas;
    
    [HideInInspector] public int init_tapeCounter;
    [HideInInspector] public int init_consumableCounter;
    [HideInInspector] public int init_childCounter;
    int current_tapeCounter;
    int current_consumableCounter;
    int current_childCounter;
    public bool isCreatorMode;


    private void Start() 
    {
        if(!isCreatorMode)
        {
            int rewarded_Consumables = menu_Handler.GetScore();
            SetConsumableCount(rewarded_Consumables);
        }
    }


    public void SetChildrenCount(int count)
    {
        init_childCounter = count;
        current_childCounter = init_childCounter;
    }
    public void DecreaseChildCount()
    {
        current_childCounter--;
        if(current_childCounter == 0) menu_Handler.GameOver(false);
    }
    public int GetChildCount()
    {
        return current_childCounter;
    }
    void ResetChildCount()
    {
        current_childCounter = init_childCounter;
        spawner.childrenToSpawn = init_childCounter;
    }



    public void SetTapeCount(int count)
    {
        init_tapeCounter = count;
        current_tapeCounter = init_tapeCounter;
        canvas.SetTapeCounter_Txt(current_tapeCounter.ToString());
    }
    public void DecreaseTapeCount() 
    {
        current_tapeCounter--;
        canvas.SetTapeCounter_Txt(current_tapeCounter.ToString());
    }
    public void IncreaseTapeCount() 
    {
        current_tapeCounter++;
        canvas.SetTapeCounter_Txt(current_tapeCounter.ToString());
    }
    void ResetTapeCount()
    {
        current_tapeCounter = init_tapeCounter;
        canvas.SetTapeCounter_Txt(current_tapeCounter.ToString());
    }
    public int GetTapeCount()
    {
        return current_tapeCounter;
    }




    public void SetConsumableCount(int count)
    {
        init_consumableCounter = count;
        current_consumableCounter = init_consumableCounter;
        canvas.SetConsumableCounter_Txt(current_consumableCounter.ToString());
    }

    public void InstantiateConsumable(GameObject pref_consumable, Vector3 inst_Pos)
    {
        if(current_consumableCounter > 0)
        {
            Instantiate(pref_consumable, inst_Pos, Quaternion.identity); 
            DecreaseConsumableCount();
            menu_Handler.DecreaseScore();
        }
    }

    public void DecreaseConsumableCount() 
    {
        current_consumableCounter--;
        canvas.SetConsumableCounter_Txt(current_consumableCounter.ToString());
    }
    public void IncreaseConsumableCount() 
    {
        current_consumableCounter++;
        canvas.SetConsumableCounter_Txt(current_consumableCounter.ToString());
    }
    void ResetConsumableCount()
    {
        current_consumableCounter = init_tapeCounter;
        canvas.SetConsumableCounter_Txt(current_consumableCounter.ToString());
    }
    public int GetConsumableCount()
    {
        return current_consumableCounter;
    }


    public void Reset()
    {
        ResetChildCount();
        ResetConsumableCount();
        ResetTapeCount();
    }
}
