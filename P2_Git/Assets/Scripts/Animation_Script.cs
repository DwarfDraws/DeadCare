using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Script : MonoBehaviour
{
    Animator anim;
    string animation_Start = "start";
    string animation_Speed = "animation_speed";
    float speed;
    
    private void Awake() 
    {
        anim = this.GetComponent<Animator>();
        speed = 1.0f;
    }

    public void SetAnimationSpeed(float targetSpeed)
    {
        float multiplicator;
        float normal_AnimationLength = anim.runtimeAnimatorController.animationClips[0].length;

        multiplicator = normal_AnimationLength / targetSpeed;
        speed *= multiplicator;
        anim.SetFloat(animation_Speed, speed);
    }
    
    public void PlayAnimation(bool isPlaying)
    {
        anim.SetBool(animation_Start, isPlaying);
    }

    public void RewindAnimation()
    {
        speed *= -1;
        anim.SetFloat(animation_Speed, speed);
    }

}
