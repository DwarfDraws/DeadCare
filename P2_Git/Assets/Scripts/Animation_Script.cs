using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Script : MonoBehaviour
{
    Animator anim;

    List<string> object_anim_IdleBools = new List<string>();
    List<string> object_anim_DeathBools = new List<string>();
    List<string> children_anim_IdleBools = new List<string>();
    List<string> children_anim_DeathBools = new List<string>();
    float speed;
    float normal_AnimationLength_seconds;
    bool hasObject_anims;
    bool hasChild_anims;

    string animation_Speed = "animation_speed";
    string animationBool_isWalking_name = "isWalking";

    
    private void Awake() 
    {
        anim = GetComponentInChildren<Animator>();

        speed = 1.0f;

        //children-anims
        children_anim_IdleBools.Add("anim_c_Wardrobe_Idle");
        children_anim_IdleBools.Add("anim_removeTape");

        children_anim_DeathBools.Add("anim_c_Wardrobe_Death");

        //object-anims
        //object_anim_IdleBools.Add("anim_o_Wardrobe_Idle");
        
        //object_anim_DeathBools.Add("anim_o_Wardrobe_Death");


        if(children_anim_IdleBools.Count > 0) hasChild_anims = true;
        if(object_anim_IdleBools.Count > 0) hasObject_anims = true;
    }

    public void PlayAnimation(int anim_index, bool isPlaying, bool isObjectAnimation, bool isDeathAnimation)
    {
        Debug.Log("Hallo hier 1 debug");
        string animation_bool = "";

        if(!isDeathAnimation)
        {
            List<string> o_anim = object_anim_IdleBools;
            List<string> c_anim = children_anim_IdleBools;
            
            if(isObjectAnimation && hasObject_anims)            animation_bool = o_anim[anim_index];
            else if (!isObjectAnimation && hasChild_anims)      animation_bool = c_anim[anim_index];
        }
        else
        {
            List<string> o_anim = object_anim_DeathBools;
            List<string> c_anim = children_anim_IdleBools;

            if(isObjectAnimation &&  o_anim.Count-1 < anim_index) return; 
            else if(!isObjectAnimation &&  c_anim.Count-1 < anim_index) return; 
            
            else animation_bool = "timerup";       
        }

        if(animation_bool != "")
        {
            Debug.Log(animation_bool);
            anim.SetBool(animation_bool, isPlaying);
            //Debug.Log(animation_bool + " " + isPlaying);
        }
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


    public void SetAnimationSpeed(int anim_index, float targetSpeed_seconds, bool isObjectAnimation)
    {
        speed = 1f; //reset to default value

        if(isObjectAnimation)   normal_AnimationLength_seconds = Get_AnimClipLength(object_anim_IdleBools, anim_index);
        else                    normal_AnimationLength_seconds = Get_AnimClipLength(children_anim_IdleBools, anim_index);

        float numOfLoops_withBias = targetSpeed_seconds / normal_AnimationLength_seconds;
        float numOfLoops = (int)numOfLoops_withBias;
        float multiply_ratio = numOfLoops / numOfLoops_withBias;

        speed *= multiply_ratio;

        anim.SetFloat(animation_Speed, speed);
    }
    
    float Get_AnimClipLength(List<string> listToCompare, int anim_index)
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

        else return 0f; //default 
    }

    public float Get_DeathAnimClipLength(int anim_index, bool isObjectAnimation)
    {
        if(isObjectAnimation) return Get_AnimClipLength(object_anim_DeathBools, anim_index);
        else return Get_AnimClipLength(children_anim_DeathBools, anim_index);
    }


    public void ResetChildAnimations()
    {
        foreach(string animation_bool in children_anim_IdleBools)
        {
            anim.SetBool(animation_bool, false);
        }

        PlayWalkingAnimation(true);
    }
}
