using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Script : MonoBehaviour
{
    Animator anim;
    
    string animation_Start = "start";
    
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        Debug.Log(this.gameObject.name + " playanimation");
        anim.Play(animation_Start);
    }

    void RewindAnimation()
    {
        if(anim.speed == 1)
        {
            anim.speed = -1;
        } 
        else anim.speed = 1;
    }
}
