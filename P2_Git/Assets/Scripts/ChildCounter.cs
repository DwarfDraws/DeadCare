using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCounter : MonoBehaviour
{
    Animator door_animator;
    static int childCount;
    static int childrenAmount;

    string door_name = "door";

    private void Start() 
    {
        ResetChildCount();
        door_animator = GameObject.Find(door_name).GetComponent<Animator>();
    }

    public void IncreaseChildCount()
    {
        childCount++;
        //Debug.Log("childCount: " + childCount + " childrenAmount: " + childrenAmount);
        if(childCount == childrenAmount) door_animator.SetTrigger("türZu"); //Play door close
    }

    public void SetChildrenAmount(int amount)
    {
        childrenAmount = amount;
    }

    public void ResetChildCount()
    {
        childCount = 0;
    }
}
