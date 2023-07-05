using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Children : MonoBehaviour
{

    Settings_script settings;
    Gameplay gameplay;
    

    [HideInInspector] public Animation_Script animation_script;
    [HideInInspector] public List<Target> tutorialTargets;
    [HideInInspector] public Target currentTarget;
    Target tempOldTarget;
    NavMesh navMesh;
    NavMeshAgent attachedAgent; 
    GameObject triggerObject;


    int tutorialIndex;
    float waitTime_seconds; 
    float beginTimer;
    float standStill_velocityThreshold;
    float init_WaitTime;
    bool startTimer;
    bool isPathInitialized;
    bool isInSafeZone;
    bool isTargetDetected, isWidgetInstantiated;
    bool waitForAgentToStop;

    string settings_name = "Settings";
    string gamplayHandler_name = "Gameplay_Handler";
    string navMeshHandler_name = "NavMesh_Handler";
    string consumableRadius_tag = "consumable_radius";
    string target_tag = "target";
    string childrenCounter_tag = "childrenCounter";
    

    private void Start() 
    {
        tutorialIndex = 0;
        init_WaitTime = 0.05f;
        standStill_velocityThreshold = 0.01f;
        currentTarget = null;
        
        settings = GameObject.Find(settings_name).GetComponent<Settings_script>();
        gameplay = GameObject.Find(gamplayHandler_name).GetComponent<Gameplay>();
 
        attachedAgent = GetComponent<NavMeshAgent>();
        animation_script = GetComponent<Animation_Script>();
        navMesh = GameObject.Find(navMeshHandler_name).GetComponent<NavMesh>();

        animation_script.PlayWalkingAnimation(true); 
    }

    void Update()
    {
        beginTimer += Time.deltaTime;
        if(beginTimer >= init_WaitTime && !isPathInitialized)
        {            
            if(settings.isTutorial) 
            {
                if(tutorialTargets.Count != 0) navMesh.SetSpecificPath(attachedAgent, tutorialTargets[tutorialIndex]);
                else
                {
                    Debug.Log("Error: no tutorial-target set.");
                    navMesh.StopAgent(attachedAgent);
                }
            }
            else navMesh.RecalculatePath(attachedAgent);
            isPathInitialized = true;
        }


        if(startTimer)
        {
            currentTarget.isTimerActive = true;
            currentTarget.Timer();
        }


        //wait until target-destination reached
        if(waitForAgentToStop)
        {
           if(attachedAgent.velocity.sqrMagnitude <= standStill_velocityThreshold) 
           {
                TargetTriggered(triggerObject);
                waitForAgentToStop = false;
           }
        } 
    }


    public Target CurrentTarget
    {
        get 
        { 
            return currentTarget; 
        }
        set
        {
            currentTarget = value;
            ResetTimer();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Target hitTarget = other.gameObject.GetComponent<Target>();

        if(other.tag == childrenCounter_tag) other.GetComponent<ChildCounter>().IncreaseChildCount();

        //current target
        if (other.tag == target_tag && hitTarget == currentTarget)
        {
            //Debug.Log("enter");
            triggerObject = other.gameObject;
            
            animation_script.PlayWalkingAnimation(false); 
            waitForAgentToStop = true;
            isTargetDetected = true;
        }       

        //child WALKS INTO consumable_radius
        if(other.tag == consumableRadius_tag && !isInSafeZone && !currentTarget.childDies) //Make children always stop their action for cookies when not checking 2nd statement
        {
            triggerObject = other.gameObject;
            
            Target consumableTarget = triggerObject.transform.GetComponentInParent<Target>();            

            if(consumableTarget.isOpen)
            {   
                Consumables consumable = triggerObject.GetComponentInParent<Consumables>();
                if(settings.consumablesHaveExistenceTime) consumable.StartExistenceTimer(false);

                //Destroy old-target Widget
                if (!currentTarget.isWaitTarget) currentTarget.DestroyWidget();
                isWidgetInstantiated = false;

                //Stop old-attachedObject animation
                Animation_Script object_animationScript = currentTarget.attachedObject_Animation;
                int anim_index = currentTarget.animation_Index;
                if(object_animationScript != null) object_animationScript.PlayAnimation(anim_index, false, true, false); 
                animation_script.PlayAnimation(anim_index, false, false, false);

                //override target
                CurrentTarget = consumableTarget;
                navMesh.SetSpecificPath(attachedAgent, consumableTarget);


                consumableTarget.isOpen = false;
                isTargetDetected = false;
            }            
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        Target hitTarget = other.gameObject.GetComponent<Target>();

        //child IS in the Consumable, but did not detect it yet
        if (other.tag == target_tag && hitTarget == currentTarget && !isTargetDetected)
        {
            Debug.Log("stay");
            triggerObject = other.gameObject;

            animation_script.PlayWalkingAnimation(false); 
            waitForAgentToStop = true;
            isTargetDetected = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        //Debug.Log("exit");
        IsSafeZone(false);
        animation_script.PlayWalkingAnimation(true); 
    }


    void TargetTriggered(GameObject triggerObject)
    {

            Target target = triggerObject.GetComponent<Target>();
            Object_attributes attachedObject = target.attachedObject;

            target.SetCurrentChild(this);
        
            

            //look at object
            if(attachedObject != null)
            {
                Vector3 lookAt_direction = attachedObject.transform.position;
                this.transform.LookAt(lookAt_direction, Vector3.up);
            }

            Color color;
            Vector3 widget_pos = triggerObject.transform.GetChild(0).gameObject.transform.position;
            //color = Color.green;
            if (!currentTarget.isWaitTarget)
            {
                if (currentTarget.isDeadly) color = Color.red;
                else
                {
                    color = Color.green;
                    isInSafeZone = true;
                }

                if(!isWidgetInstantiated) 
                {
                    if(!currentTarget.isConsumable) currentTarget.InstantiateWidget_target(widget_pos, color);
                    else if (settings.showConsumableTimer) currentTarget.InstantiateWidget_target(widget_pos, color);
                    isWidgetInstantiated = true;
                }
            }

            currentTarget.isOpen = false;
            startTimer = true;
            
            //animations
            //normal anim
            if(attachedObject != null)
            {
                if(!attachedObject.isTaped && !target.isWaitTarget)
                {
                    animation_script.SetAnimationSpeed(target.animation_Index, target.waitTime_seconds, false);
                    animation_script.PlayAnimation(target.animation_Index, true, false, false);
                    currentTarget.Animate_AttachedObject(); 
                }
                //taperemove
                else if(attachedObject.isTaped)
                {
                    animation_script.PlayTapeRemoveAnimation(true);
                }
            }
            //consumable
            else if(target.isConsumable)
            {
                animation_script.ResetChildAnimations();
                animation_script.PlayWalkingAnimation(false);
                animation_script.PlayConsumableAnimation(true);
            }
            //wait-target
            else
            {
                animation_script.PlayWalkingAnimation(false);
            }
    }
  


    public void Reset()
    {
        if(settings.isTutorial)
        {
            tutorialIndex++;
            if(tutorialIndex < tutorialTargets.Count)
                navMesh.SetSpecificPath(attachedAgent, tutorialTargets[tutorialIndex]);
            else Debug.Log("Tutorial finished");
        }

        else if(currentTarget.isConsumable)
        {
            Destroy(currentTarget.gameObject);
            navMesh.RecalculatePath(attachedAgent);
        }
        
        else
        {
            tempOldTarget = currentTarget;
            navMesh.RecalculatePath(attachedAgent);

            if(!attachedAgent.isStopped)
            {
                navMesh.Un_target(tempOldTarget);
            }
        }
        
        animation_script.ResetChildAnimations();
        IsSafeZone(false);
        isTargetDetected = false;
        isWidgetInstantiated = false;
        ResetTimer();

    }

    public void ResetTimer()
    {
        startTimer = false;
    }


    void IsSafeZone(bool isActivated)
    {
        if (isActivated) isInSafeZone = true;
        else isInSafeZone = false;
    }
    

    public void ChildDestroy()
    {
        navMesh.Remove_Agent(GetComponent<NavMeshAgent>());
        gameplay.DecreaseChildCount();
        Destroy(gameObject); 
    }
}
