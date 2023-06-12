using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Children : MonoBehaviour
{

    Settings_script settings;
    Canvas_Script canvas;
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
    bool isTargetDetected;
    

    private void Start() 
    {
        tutorialIndex = 0;
        
        settings = GameObject.Find("Settings").GetComponent<Settings_script>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas_Script>();
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

        if(startTimer) currentTarget.Timer(this);
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
                Consumables consumable = triggerObject.GetComponentInParent<Consumables>();

                currentTarget.DestroyWidget();
                isTargetDetected = false;
                Debug.Log("consumable_radius entered");
                if (currentTarget.attachedObject != null) currentTarget.attachedObject.Animate(1); //reset animation

                SetTarget(consumableTarget);
                navMesh.SetSpecificPath(attachedAgent, consumableTarget);
                if(settings.consumablesHaveExistenceTimer) consumable.StartExistenceTimer(false);

                consumableTarget.isOpen = false;
            }            
        }

        //current target
        else if (other.tag == "target" && triggerObject.GetComponent<Target>() == currentTarget)
        {
            TargetTriggered(other);
            Debug.Log("enter");
        }        
    }

    private void OnTriggerStay(Collider other) 
    {
        GameObject triggerObject = other.gameObject;

        if (other.tag == "target" && triggerObject.GetComponent<Target>() == currentTarget && !isTargetDetected)
        {
            TargetTriggered(other);
            Debug.Log("stay");
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        IsSafeZone(false);
        isStopped = false; //animator
    }

    void TargetTriggered(Collider other)
    {
            Color color;
            Vector3 widget_pos = other.gameObject.transform.GetChild(0).gameObject.transform.position;
            isStopped = true; //animator
            
            color = Color.green;
            /*
            if(currentTarget.isDeadly) color = Color.green; // TEST: Changed color to green
            else
            {
                color = Color.green;
                isInSafeZone = true;
            } 
            */
            currentTarget.InstantiateWidget(widget_pos, color);

            currentTarget.isOpen = false;
            startTimer = true;

            isTargetDetected = true;
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
        Debug.Log("child reset");
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
