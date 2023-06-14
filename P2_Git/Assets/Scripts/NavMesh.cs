using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour
{
    List<Target> targets = new List<Target>();
    [HideInInspector] public List<NavMeshAgent> agents = new List<NavMeshAgent>();
    List<NavMeshAgent> stoppedAgents = new List<NavMeshAgent>();
    NavMeshPath path;
    [SerializeField] List<GameObject> target_transforms = new List<GameObject>();

    string tag_child = "child";
    string tag_target = "target";


    private void Start() {

        path = new NavMeshPath();
        GenerateRandomSeed();
        InitAgentsList();
        InitTargets();
    }

    private void Update() 
    {
               
        //EDIT: Random walk
        //in every frame try to find a new destination for the agents if they're stopped
        if(stoppedAgents.Count > 0)
        {
            foreach(NavMeshAgent a in stoppedAgents)
            {
                //Debug.Log(stoppedAgents.Count);
                UpdateAllOpenTargets(a);
                if(GetOpenTargets().Count > 0)
                { 
                    RecalculatePath(a);
                    Debug.Log("new open Path found");
                }
            }
        }
    }


    public void RecalculateAllPaths()
    {
        foreach(NavMeshAgent a in agents) RecalculatePath(a);
    }

    //gets also called when mousebutton is released after dragging dynamic obstacle
    public void RecalculatePath(NavMeshAgent agent){ 
        
        if(agents != null){

            Children children = agent.gameObject.GetComponent<Children>();
            List<Target> openTargets; //EDIT: Make global?
            int targetIndex;
            Vector3 destination;

            UpdateAllOpenTargets(agent);
            Update_AllTargeted();
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
                if (path.status == NavMeshPathStatus.PathComplete){ //EDIT: already calculated in UpdateAllOpenTargets(agent); ?
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


    //For specific targets
    public void SetSpecificPath(NavMeshAgent agent, Target target){
        UpdateAllOpenTargets(agent);
        
        if(target.isOpen)
        {  
            Children children = agent.gameObject.GetComponent<Children>();
            Vector3 destination;
            destination = target.transform.position;

            //Calculates the Path
            if (agent.CalculatePath(destination, path)) 
            {         
                //sets navMesh Path   
                if (path.status == NavMeshPathStatus.PathComplete){ 
                    agent.destination = destination;
                    agent.isStopped = false;
                    target.isOpen = false;

                    children.SetTarget(target);
                }           
            }
        }
    }



    //Agent
    void InitAgentsList()
    {
        foreach(GameObject a in GameObject.FindGameObjectsWithTag(tag_child)){
            agents.Add(a.GetComponent<NavMeshAgent>());
        }
    }
    public void Add_Agent(NavMeshAgent a)
    {
        agents.Add(a);
        RecalculatePath(a);
    }
    public void Remove_Agent(NavMeshAgent a)
    {
        agents.Remove(a);
        if(stoppedAgents.Contains(a)) stoppedAgents.Remove(a);
    }
    public void Clear_AgentList()
    {
        agents.Clear();
    }

    public void StopAgent(NavMeshAgent agent)
    {
        Debug.Log("StopAgent(): " + agent.name);
        agent.isStopped = true;
        stoppedAgents.Add(agent);
    }




    //Targets
    void InitTargets()
    {
        foreach(GameObject t in GameObject.FindGameObjectsWithTag(tag_target))
        {
            target_transforms.Add(t);
            targets.Add(t.GetComponent<Target>());
        }
    }
    public void UpdateAllOpenTargets(NavMeshAgent agent) //EDIT THIS: MUST BE CALC FOR ALL AGENTS
    {
        foreach(Target target in targets)
        {
            if (agent.CalculatePath(target.gameObject.transform.position, path)){ //checks all reachable targets
                if (path.status == NavMeshPathStatus.PathComplete){
                    target.isOpen = true;
                }
                else target.isOpen = false;
            }
        }
    }
    public void Update_AllTargeted()
    {
        List<Target> currentlyTargeted_copy = GetAllCurrentlyTargeted();
        
        foreach (Target t in targets) {    
            foreach (Target currentlyTargeted in currentlyTargeted_copy) {
                if(t == currentlyTargeted){
                    t.isTargeted = true;
                    break;
                }
                else Un_target(t);
            }
        }
    }

    List<Target> GetAllCurrentlyTargeted()
    {
        List<Target> currentlyTargeted = new List<Target>();

        foreach (NavMeshAgent a in agents){
            Children children = a.gameObject.GetComponent<Children>();
            currentlyTargeted.Add(children.GetTarget());
        }
        return currentlyTargeted;
    }
    

    List<Target> GetOpenTargets()
    {
        List<Target> openTargets = new List<Target>(); 

        foreach(Target t in targets)
        {
            if (t.isOpen && !t.isTargeted) openTargets.Add(t);
        }

        return openTargets;
    }
  
    public void Un_target(Target t)
    {
        t.isTargeted = false;
    }





    //Randomization
    void GenerateRandomSeed()
    {
        int tempSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(tempSeed);
    }

    int RandomTargetIndex(List<Target> openTargets)
    {
        int targetIndex = Random.Range(0, openTargets.Count);
        //Debug.Log(target_Index);
        return targetIndex;
    }

}
