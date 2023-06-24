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
    bool isPathInitialized;
    bool startTimer;
    bool isInSafeZone;
    bool isTargetDetected, isWidgetInstantiated;
    bool waitForAgentToStop;

    string settings_name = "Settings";
    string gamplayHandler_name = "Gameplay_Handler";
    string navMeshHandler_name = "NavMesh_Handler";

    string tag_consumableRadius = "consumable_radius";
    string tag_target = "target";
    

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


        if(startTimer) currentTarget.Timer();


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

        //current target
        if (other.tag == tag_target && hitTarget == currentTarget)
        {
            //Debug.Log("enter");
            triggerObject = other.gameObject;
            
            animation_script.PlayWalkingAnimation(false); 
            waitForAgentToStop = true;
            isTargetDetected = true;
        }       

        //consumable
        if(other.tag == tag_consumableRadius /*&& !isInSafeZone*/) //Make children always stop their action for cookies
        {
            triggerObject = other.gameObject;
            Target consumableTarget = triggerObject.transform.GetComponentInParent<Target>();

            if(consumableTarget.isOpen)
            {   
                Consumables consumable = triggerObject.GetComponentInParent<Consumables>();
                Animation_Script object_animationScript = currentTarget.attachedObject_Animation;
                int anim_index = currentTarget.animation_Index;
                isWidgetInstantiated = false;

                if (!currentTarget.isWaitTarget) currentTarget.DestroyWidget();

                if(object_animationScript != null) object_animationScript.PlayAnimation(anim_index, false, true, false); //reset animation
                animation_script.PlayAnimation(anim_index, false, false, false);

                CurrentTarget = consumableTarget;
                navMesh.SetSpecificPath(attachedAgent, consumableTarget);

                if(settings.consumablesHaveExistenceTimer) consumable.StartExistenceTimer(false);

                consumableTarget.isOpen = false;
                isTargetDetected = false;
            }            
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        Target hitTarget = other.gameObject.GetComponent<Target>();

        //current target
        if (other.tag == tag_target && hitTarget == currentTarget && !isTargetDetected)
        {
            //Debug.Log("stay");
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

            target.SetCurrentChild(this);
        
            

            //look at object
            if(target.attachedObject != null)
            {
                Vector3 lookAt_direction = target.attachedObject.transform.position;
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
                    currentTarget.InstantiateWidget_target(widget_pos, color);
                    isWidgetInstantiated = true;
                }
            }

            currentTarget.isOpen = false;
            startTimer = true;
            animation_script.SetAnimationSpeed(target.animation_Index, target.waitTime_seconds, false);
            animation_script.PlayAnimation(target.animation_Index, true, false, false);
            currentTarget.Animate_AttachedObject(); 
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
