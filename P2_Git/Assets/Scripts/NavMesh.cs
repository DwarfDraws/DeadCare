using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour
{
    public List<NavMeshAgent> agents = new List<NavMeshAgent>();
    List<NavMeshAgent> stoppedAgents = new List<NavMeshAgent>();
    List<Target> targets = new List<Target>();
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
                UpdateAllOpenTargets(a);
                if(GetOpenTargets().Count > 0)
                { 
                    RecalculatePath(a);
                    Debug.Log("new open Path found!");
                }
            }
        }
    }


    public void RecalculateAllPaths(){
        foreach(NavMeshAgent a in agents) RecalculatePath(a);
    }

    //gets also called when mousebutton is released after dragging dynamic obstacle
    public void RecalculatePath(NavMeshAgent agent){ 
        
        if(agents != null){

            Children children = agent.gameObject.GetComponent<Children>();
            List<Target> openTargets;
            Vector3 destination;
            int targetIndex;

            UpdateAllOpenTargets(agent);
            openTargets = GetOpenTargets();

            if(openTargets.Count == 0)
            {
                StopAgent(agent);
                return;
            }

            targetIndex = RandomTargetIndex(openTargets);
            destination = openTargets[targetIndex].gameObject.transform.position;

            //Calculates the Path
            if (agent.CalculatePath(destination, path)) 
            {         
                //sets navMesh Path   
                if (path.status == NavMeshPathStatus.PathComplete){
                    agent.destination = destination;
                    agent.isStopped = false;


                    openTargets[targetIndex].isTargeted = true;
                    children.SetTarget(openTargets[targetIndex]);
                    
                    //Debug.Log(agent.name + " target: " + openTargets[targetIndex].name);
                }           
            }
        }
        else Debug.Log("ALL CHILDREN DEAD!");
    }

    void StopAgent(NavMeshAgent agent){
        //Debug.Log("StopAgent(): " + agent.name);
        agent.isStopped = true;
        stoppedAgents.Add(agent);
    }


    //updates isOpen-attribute for all targets
    public void UpdateAllOpenTargets(NavMeshAgent agent){
        
        foreach(Target target in targets){
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
    List<Target> GetOpenTargets(){
        List<Target> openTargets = new List<Target>(); 

        foreach(Target t in targets){
            if (t.isOpen && !t.isTargeted) openTargets.Add(t);
        }

        return openTargets;
    }



    void InitTargets(){

        foreach(GameObject t in GameObject.FindGameObjectsWithTag("target")){
            target_transforms.Add(t);
            targets.Add(t.GetComponent<Target>());
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
        if(stoppedAgents.Contains(a)) stoppedAgents.Remove(a);
    }

    int RandomTargetIndex(List<Target> openTargets){
        int targetIndex = Random.Range(0, openTargets.Count);
        //Debug.Log(target_Index);
        return targetIndex;
    }
   
    
    public void Update_AllTargeted(){
        foreach (Target t in targets) {    
            foreach (Target currentlyTargeted in GetAllCurrentlyTargeted()) 
                if(t != currentlyTargeted) Un_target(t);
        }
    }

    List<Target> GetAllCurrentlyTargeted(){
        List<Target> currentlyTargeted = new List<Target>();

        foreach (NavMeshAgent a in agents){
            Children children = a.gameObject.GetComponent<Children>();
            currentlyTargeted.Add(children.GetTarget());
        }

        return currentlyTargeted;
    }
    
    public void Un_target(Target t){
        t.isTargeted = false;
    }
    
}
