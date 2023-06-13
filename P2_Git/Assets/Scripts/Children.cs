using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Children : MonoBehaviour
{

    Settings_script settings;
    //Canvas_Script canvas;
    Gameplay gameplay;
    
    [SerializeField] Animator animator;
    [SerializeField] List<Target> tutorialTargets;
    public Target currentTarget;
    Target tempOldTarget;
    NavMesh navMesh;
    
    NavMeshAgent attachedAgent; 

    int tutorialIndex;
    float waitTime_seconds; 
    float beginTimer;
    public bool isStopped;
    bool isPathInitialized;
    bool startTimer;
    bool isInSafeZone;
    bool isTargetDetected, isWidgetInstantiated;

    string canvas_name = "InGameUI";
    

    private void Start() 
    {
        tutorialIndex = 0;
        
        settings = GameObject.Find("Settings").GetComponent<Settings_script>();
        //canvas = GameObject.Find(canvas_name).GetComponent<Canvas_Script>();
        gameplay = GameObject.Find("Gameplay_Handler").GetComponent<Gameplay>();
 
        attachedAgent = this.GetComponent<NavMeshAgent>();
        currentTarget = null;
        navMesh = GameObject.Find("NavMesh_Handler").GetComponent<NavMesh>();

    }

    void Update()
    {
        
        //animator
        animator.SetBool("isidle", isStopped);
        //animator
        

        beginTimer += Time.deltaTime;
        if(beginTimer >= 0.05 && !isPathInitialized)
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

        if(startTimer)
        {
            currentTarget.Timer(this);
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
        GameObject triggerObject = other.gameObject;
        
        //consumable
        if(other.tag == "consumable_radius" && !isInSafeZone)
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
                
                if (currentTarget.attachedObject_Animation != null) currentTarget.attachedObject_Animation.PlayAnimation(currentTarget.animation_Index, false); //reset animation
                SetTarget(consumableTarget);

                navMesh.SetSpecificPath(attachedAgent, consumableTarget);
                if(settings.consumablesHaveExistenceTimer) consumable.StartExistenceTimer(false);

                consumableTarget.isOpen = false;
                isTargetDetected = false;
            }            
        }

        //current target
        if (other.tag == "target" && triggerObject.GetComponent<Target>() == currentTarget)
        {
            //Debug.Log("enter");
            TargetTriggered(other);
        }        
    }

    private void OnTriggerStay(Collider other) 
    {
        GameObject triggerObject = other.gameObject;

        if (other.tag == "target" && triggerObject.GetComponent<Target>() == currentTarget && !isTargetDetected)
        {
            //Debug.Log("stay");
            TargetTriggered(other);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        IsSafeZone(false);
        isStopped = false; //animator
    }



    void TargetTriggered(Collider other)
    {
            isTargetDetected = true;
            isStopped = true; //animator
            
            Color color;
            Vector3 widget_pos = other.gameObject.transform.GetChild(0).gameObject.transform.position;
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
    

    public void ChildDestroy(){
        navMesh.Remove_Agent(this.GetComponent<NavMeshAgent>());
        gameplay.DecreaseChildCount();
        Destroy(gameObject); 
    }
}
