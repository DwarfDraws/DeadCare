using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Script : MonoBehaviour
{
    Animator anim;

    List<string> objectAnimation_Bools = new List<string>();
    List<string> childrenAnimation_Bools = new List<string>();
    float speed;
    float normal_AnimationLength;

    string animation_Speed = "animation_speed";
    string animationBool_isWalking_name = "isWalking";

    
    private void Awake() 
    {
        anim = GetComponent<Animator>();
        speed = 1.0f;

        childrenAnimation_Bools.Add("anim_removeTape");
        childrenAnimation_Bools.Add("anim_removeTape");

        objectAnimation_Bools.Add("anim_schrankFällt");
        objectAnimation_Bools.Add("anim_schrankDreht");
    }

    public void SetAnimationSpeed(int anim_index, float targetSpeed, bool isObjectAnimation)
    {
        speed = 1f; //reset to default value

        if(isObjectAnimation)   normal_AnimationLength = TraverseAnimationClips(objectAnimation_Bools, anim_index);
        else                    normal_AnimationLength = TraverseAnimationClips(childrenAnimation_Bools, anim_index);

        float multiply_ratio = normal_AnimationLength / targetSpeed;
        speed *= multiply_ratio;

        anim.SetFloat(animation_Speed, speed);
    }
    
    public void PlayAnimation(int anim_index, bool isPlaying, bool isObjectAnimation)
    {
        string animation_bool;

        if(isObjectAnimation)      animation_bool = objectAnimation_Bools[anim_index];
        else                       animation_bool = childrenAnimation_Bools[anim_index];

        if(animation_bool != "") anim.SetBool(animation_bool, isPlaying);
    }

    public void PlayWalkingAnimation(bool isPlaying)
    {
        anim.SetBool(animationBool_isWalking_name, isPlaying);
    }

    public void RewindAnimation()
    {
        speed *= -1f;
        anim.SetFloat(animation_Speed, speed);
    }


    float TraverseAnimationClips(List<string> listToCompare, int anim_index)
    {
        //check if index can be reached
        if(listToCompare.Count > anim_index)
        { 
            AnimationClip[] avaiable_animClips = anim.runtimeAnimatorController.animationClips;
            string animation_name = listToCompare[anim_index];
            int i = 0;

            //find the matching animation-clip 
            while(animation_name != avaiable_animClips[i].name) { i++; }

            return anim.runtimeAnimatorController.animationClips[i].length;
        }

        else return 1f; //default 
    }


    public void ResetChildAnimations()
    {
        foreach(string animation_bool in childrenAnimation_Bools)
        {
            anim.SetBool(animation_bool, false);
        }
        PlayWalkingAnimation(true);
    }
}
