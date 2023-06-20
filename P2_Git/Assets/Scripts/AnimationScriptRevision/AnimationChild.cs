using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationChild : MonoBehaviour
{
    private Animator m_Animator;
    private float m_waitTime;
    private bool isAnimObject;

    public List<IdleSO> idles;

    private void Start() {
        m_Animator = GetComponent<Animator>();
    }

    public void PlayAnimation(int index){
        m_Animator.runtimeAnimatorController = idles[index].animatorOV;
        m_Animator.Play("Idle");
        loopHandler();
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



