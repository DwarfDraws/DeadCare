using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour
{
    public List<NavMeshAgent> agents = new List<NavMeshAgent>();
    List<NavMeshAgent> stoppedAgents = new List<NavMeshAgent>();
    List<Target_attributes> targets = new List<Target_attributes>();
    [SerializeField] List<GameObject> target_transforms = new List<GameObject>();
    
    NavMeshPath path;

    private bool isPathInitialized;
    float timer;


    private void Start() {
        timer = 0.0f;
        isPathInitialized = false;  

        path = new NavMeshPath();
        GenerateRandomSeed();
        InitTargets();
        InitAgentsList();
    }

    private void Update() {
        
        //Initializing Path        
        //Waiting for dynamic obstacles to finish calculating before checking Path (could be improved) 
        timer += Time.deltaTime;
        if(timer >= 0.05 && !isPathInitialized){            
            foreach (NavMeshAgent a in agents) RecalculatePath(a);
            isPathInitialized = true;
        }

        //in every frame try to find a new destination for the agents if they're stopped
        if(stoppedAgents.Count > 0){
            List<NavMeshAgent> stoppedAgents_Copy = stoppedAgents;
            foreach(NavMeshAgent a in stoppedAgents_Copy){
                //Debug.Log(stoppedAgents.Count);
                UpdateOpenTargets(a);
                if(GetOpenTargets().Count > 0)
                { 
                    RecalculatePath(a);
                    Debug.Log("new open Path found!");
                }
                //else Debug.Log("no new target found");
            }
        }
    }


    public void RecalculateAllPaths(){
        foreach(NavMeshAgent a in agents) RecalculatePath(a);
    }

    //gets also called when mousebutton is released after dragging dynamic obstacle
    public void RecalculatePath(NavMeshAgent agent){ 
        
        if(agents != null){

            List<Target_attributes> openTargets;
            Vector3 destination;
            int targetIndex;

            UpdateOpenTargets(agent);
            openTargets = GetOpenTargets();

            if(openTargets.Count == 0){
                Debug.Log("no open targets found");
                StopAgent(agent);
                return;
            }

            targetIndex = RandomTargetIndex(openTargets);
            destination = openTargets[targetIndex].gameObject.transform.position;
            //Debug.Log("new Target");

            //Calculates the Path
            if (agent.CalculatePath(destination, path)) 
            {         
                //sets navMesh Path   
                if (path.status == NavMeshPathStatus.PathComplete){
                    agent.destination = destination;
                    agent.isStopped = false;


                    openTargets[targetIndex].isTargeted = true;
                    agent.gameObject.GetComponent<Children>().SetTarget(openTargets[targetIndex]);
                    Debug.Log(agent.name + " target: " + openTargets[targetIndex].name);
                } 

                //if no Path is avaiable
                else{
                    StopAgent(agent);
                } 

                //isPathInitialized = true; //ends Update-Loop
            
            }
        }
        else Debug.Log("ALL CHILDREN DEAD!");
    }

    void StopAgent(NavMeshAgent agent){
        Debug.Log("StopAgent(): " + agent.name);
        agent.isStopped = true;
        stoppedAgents.Add(agent);
    }


    //updates isOpen-attribute for all targets
    public void UpdateOpenTargets(NavMeshAgent agent){
        
        foreach(Target_attributes target in targets){
            if (agent.CalculatePath(target.gameObject.transform.position, path)){
                //Debug.Log(target.name + ": " + target.isOneCurrentDestination);
                if (path.status == NavMeshPathStatus.PathComplete){
                    target.isOpen = true;
                }
                else target.isOpen = false;
            }
        }
    }

    //give's all open targets !excluding targets targeted by children!
    List<Target_attributes> GetOpenTargets(){
        List<Target_attributes> openTargets = new List<Target_attributes>(); 

        foreach(Target_attributes t in targets){
            if (t.isOpen && !t.isTargeted) openTargets.Add(t);
        }

        return openTargets;
    }



    void InitTargets(){

        foreach(GameObject t in GameObject.FindGameObjectsWithTag("target")){
            target_transforms.Add(t);
            targets.Add(t.GetComponent<Target_attributes>());
        }
    }
    void InitAgentsList(){

        foreach(GameObject a in GameObject.FindGameObjectsWithTag("child")){
            agents.Add(a.GetComponent<NavMeshAgent>());
        }
    }
    void GenerateRandomSeed(){
        int tempSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(tempSeed);
    }

    public void AddAgent(NavMeshAgent a){
        agents.Add(a);
        RecalculatePath(a);
    }
    public void RemoveAgent(NavMeshAgent a){
        agents.Remove(a);
    }

    int RandomTargetIndex(List<Target_attributes> openTargets){
        int targetIndex = Random.Range(0, openTargets.Count);
        //Debug.Log(target_Index);
        return targetIndex;
    }
   
    
    public void Update_AllTargeted(){
        foreach (Target_attributes t in targets) 
            foreach (NavMeshAgent a in agents){
                if(a.gameObject.GetComponent<Children>().GetTarget() != t) Un_target(t);
            };
    }
    
    //gets called when children exits Zone again
    public void Un_target(Target_attributes t){
        t.isTargeted = false;
    }
    
}
