using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Children : MonoBehaviour
{

    Settings_script settings;
    Gameplay gameplay;
    
    [SerializeField] Animator animator;
    [HideInInspector] public Animation_Script animation_Script;
    [SerializeField] List<Target> tutorialTargets;
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
    [HideInInspector] public bool isStopped;
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
        animation_Script = GetComponent<Animation_Script>();
        navMesh = GameObject.Find(navMeshHandler_name).GetComponent<NavMesh>();

    }

    void Update()
    {
        
        //animator
        animator.SetBool("isidle", isStopped);
        //animator
        

        beginTimer += Time.deltaTime;
        if(beginTimer >= init_WaitTime && !isPathInitialized)
        {            
            if(settings.isTutorial) 
            {
                if(tutorialTargets.Count != 0) navMesh.SetSpecificPath(attachedAgent, tutorialTargets[tutorialIndex]);
                else
                {
                    Debug.Log("!!!! no tutorial-target set !!!!");
                    navMesh.StopAgent(attachedAgent);
                }
            }
            else navMesh.RecalculatePath(attachedAgent);
            isPathInitialized = true;
        }


        if(startTimer) currentTarget.Timer(this);


        //wait for target-destination reached
        if(waitForAgentToStop)
        {
           if(attachedAgent.velocity.sqrMagnitude <= standStill_velocityThreshold) 
           {
                TargetTriggered(triggerObject);
                waitForAgentToStop = false;
           }
        } 
    }


    public void SetTarget(Target t)
    {
        currentTarget = t;
        ResetTimer();
    }

    public Target GetTarget() 
    { 
        return currentTarget; 
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerObject = other.gameObject;
        
        //consumable
        if(other.tag == tag_consumableRadius && !isInSafeZone)
        {
            Target consumableTarget = triggerObject.transform.GetComponentInParent<Target>();
            if(consumableTarget.isOpen)
            {   
                isWidgetInstantiated = false;
                
                Consumables consumable = triggerObject.GetComponentInParent<Consumables>();


                if (!currentTarget.isWaitTarget)
                {
                    currentTarget.DestroyWidget();
                }
                
                if (currentTarget.attachedObject_Animation != null) currentTarget.attachedObject_Animation.PlayAnimation(currentTarget.animation_Index, false, true); //reset animation
                SetTarget(consumableTarget);

                navMesh.SetSpecificPath(attachedAgent, consumableTarget);
                if(settings.consumablesHaveExistenceTimer) consumable.StartExistenceTimer(false);

                consumableTarget.isOpen = false;
                isTargetDetected = false;
            }            
        }

        //current target
        if (other.tag == tag_target && triggerObject.GetComponent<Target>() == currentTarget)
        {
            //Debug.Log("enter");
            waitForAgentToStop = true;
            isTargetDetected = true;
        }        
    }

    private void OnTriggerStay(Collider other) 
    {
        triggerObject = other.gameObject;

        if (other.tag == tag_target && triggerObject.GetComponent<Target>() == currentTarget && !isTargetDetected)
        {
            //Debug.Log("stay");
            waitForAgentToStop = true;
            isTargetDetected = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        IsSafeZone(false);
        isStopped = false; //animator
    }



    void TargetTriggered(GameObject triggerObject)
    {
            isStopped = true; //animator

            Target target = triggerObject.GetComponent<Target>();
            target.SetCurrentChild(this);
        
            animation_Script.SetAnimationSpeed(target.waitTime_seconds);
            animation_Script.PlayAnimation(target.animation_Index, true, false);

            if(target.attachedObject != null)
            {
                Vector3 lookAtObject = target.attachedObject.transform.position;
                this.transform.LookAt(lookAtObject, Vector3.up);
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
        navMesh.Remove_Agent(this.GetComponent<NavMeshAgent>());
        gameplay.DecreaseChildCount();
        Destroy(gameObject); 
    }
}
