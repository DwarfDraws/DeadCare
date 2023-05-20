using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Children : MonoBehaviour
{

    Settings_script settings;
    Canvas_Script canvas;
    
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
    bool timerDown;
    bool startTimer;
    bool isInSafeZone;
    

    private void Start() 
    {
        tutorialIndex = 0;
        
        settings = GameObject.Find("Settings").GetComponent<Settings_script>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas_Script>();
 
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
            if(settings.isTutorial) navMesh.SetSpecificPath(attachedAgent, tutorialTargets[tutorialIndex]);
            else navMesh.RecalculatePath(attachedAgent);
            isPathInitialized = true;
        }

        if(startTimer) currentTarget.Timer(this);
    }


    public void SetTarget(Target t)
    {
        currentTarget = t;
        ResetTimer();
        //Debug.Log(currentTarget.name + " set as currentTarget");
    }

    public Target GetTarget() 
    { 
        return currentTarget; 
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject triggerObject = other.gameObject;
        
        //normal target
        if (triggerObject.GetComponent<Target>() == currentTarget)
        {
            Color color;
            Vector3 widget_pos = other.gameObject.transform.GetChild(0).gameObject.transform.position;
            isStopped = true; //animator
            
            if(currentTarget.isDeadly) color = Color.red;
            else
            {
                color = Color.green;
                isInSafeZone = true;
            } 
            currentTarget.InstantiateWidget(widget_pos, waitTime_seconds, color);

            currentTarget.isOpen = false;
            timerDown = true;
            startTimer = true;
        }


        //consumable
        else if(other.tag == "consumable_radius" && !isInSafeZone)
        {
            Target consumableTarget = triggerObject.transform.GetComponentInParent<Target>();
            if(consumableTarget.isOpen)
            {   
                Consumables consumable = triggerObject.GetComponentInParent<Consumables>();
                currentTarget.DestroyWidget();

                SetTarget(consumableTarget);
                navMesh.SetSpecificPath(attachedAgent, consumableTarget);
                if(settings.consumablesHaveExistenceTimer) consumable.StartExistenceTimer(false);

                consumableTarget.isOpen = false;
            }            
        }
        
    }

    private void OnTriggerExit(Collider other) 
    {
        IsSafeZone(false);
        isStopped = false; //animator
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

        ResetTimer();
    }

    public void ResetTimer()
    {
        startTimer = false;
        timerDown = false;
    }


    void IsSafeZone(bool isActivated)
    {
        if (isActivated) isInSafeZone = true;
        else isInSafeZone = false;
    }
    

    public void ChildDestroy(){
        navMesh.Remove_Agent(this.GetComponent<NavMeshAgent>());
        Destroy(gameObject); 
    }
}
