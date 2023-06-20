using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsObjects : MonoBehaviour
{
    private Animator m_Animator;
    private float m_waitTime;
    private bool isAnimObject;

    private int m_index;

    private void Start() {
        m_Animator = GetComponent<Animator>();
    }

    public void PlayAnimation(int index){
        m_index = index;
        if(index != 10){        // Indexe von einfachen Interactionen ohne idle oder death
            m_Animator.Play("Idle");
            loopHandler();
        }
        else{
            m_Animator.Play("SingleInteraction");
        }
    }


    public void GetWaitTimeInfo(float waitTime){
        m_waitTime = waitTime;
    }

    private void loopHandler(){
        float idleLength = m_Animator.GetCurrentAnimatorStateInfo(0).length;
        float loops = 2f/idleLength;
        m_Animator.SetFloat("IdleExitTime", loops);
    }
}



