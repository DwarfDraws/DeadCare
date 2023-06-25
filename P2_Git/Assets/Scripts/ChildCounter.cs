using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCounter : MonoBehaviour
{
    Animator door_animator;
    static int childCount;
    static int childrensAmount;

    string door_name = "door";

    private void Start() {
        door_animator = GameObject.Find(door_name).GetComponent<Animator>();
    }

    public void IncreaseChildCount()
    {
        childCount++;
        if(childCount == childrensAmount) door_animator.SetTrigger("türZu"); //Play door close
    }

    public void SetChildrenAmount(int amount)
    {
        childrensAmount = amount;
    }
}
