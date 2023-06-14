using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Script : MonoBehaviour
{
    Animator anim;

    List<string> objectAnimation_Bools = new List<string>();
    float speed;

    string animation_Speed = "animation_speed";

    
    private void Awake() 
    {
        anim = this.GetComponent<Animator>();
        speed = 1.0f;

        objectAnimation_Bools.Add("SchrankFällt");
        objectAnimation_Bools.Add("SchrankDreht");
    }

    public void SetAnimationSpeed(float targetSpeed)
    {
        float multiplicator;
        float normal_AnimationLength = anim.runtimeAnimatorController.animationClips[0].length;

        multiplicator = normal_AnimationLength / targetSpeed;
        speed *= multiplicator;
        anim.SetFloat(animation_Speed, speed);
    }
    
    public void PlayAnimation(int anim_index, bool isPlaying)
    {
        anim.SetBool(objectAnimation_Bools[anim_index], isPlaying);
    }

    public void RewindAnimation()
    {
        speed *= -1;
        anim.SetFloat(animation_Speed, speed);
    }

}
