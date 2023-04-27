using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    List<Target> targets = new List<Target>();
    [SerializeField] List<GameObject> target_transforms = new List<GameObject>();
    
    NavMeshPath path;

    public bool isPathRecalculated;
    float timer;


    private void Start() {
        timer = 0;
        isPathRecalculated = false;  
        path = new NavMeshPath();

        GenerateRandomSeed();
        InitTargets();
        
        
        //foreach(Target t in targets) Debug.Log(t.isOpen);
        //Debug.Log(targets.Count);
    }

    private void Update() {
        
        //Waiting for dynamic obstacles to finish calculating before checking Path (could be improved) 
        timer += Time.deltaTime;
        
        //Consequent Updating of Path-Finding
        //isPathRecalculated gets true if a path is found, gets false if there is no path avaiable
        if(timer >= 0.05 && !isPathRecalculated){
            Debug.Log("Update() - recalculates path...");
            RecalculatePath();
        }
    }



    //gets also called when mousebutton is released after dragging dynamic obstacle
    public void RecalculatePath(){ 
        
        List<Target> reachableTargets;
        Transform destination;
        int targetIndex;

        UpdateTarget_Reachabilities();
        reachableTargets = GetReachableTargets();

        if(reachableTargets.Count == 0){
            Debug.Log("no open targets");
            StopAgent();
            return;
        }

        targetIndex = RandomTargetIndex(reachableTargets);

        destination = reachableTargets[targetIndex].gameObject.transform;

        //Calculates the Path
        if (navMeshAgent.CalculatePath(destination.position, path)) 
        {         
            //set navMesh Path   
            if (path.status == NavMeshPathStatus.PathComplete){
                navMeshAgent.destination = destination.position;
                navMeshAgent.isStopped = false;
                
                reachableTargets[targetIndex].isReachable = false;
                reachableTargets[targetIndex].isOneCurrentDestination = true;
                Debug.Log("path recalculated, targetIndex: " + targetIndex);
            } 
            
            //if no Path is avaiable
            else{
                StopAgent();
            } 
            
            isPathRecalculated = true; //ends Update-Loop
        }
    }

    void StopAgent(){
        navMeshAgent.isStopped = true;
        Debug.Log("StopAgent() - wait for open path");
    }


    void InitTargets(){

        foreach(GameObject t in GameObject.FindGameObjectsWithTag("target")){
            target_transforms.Add(t);
            targets.Add(t.GetComponent<Target>());
        }
    }

    //resets target-state after a dynamic Obstacle was moved 
    public void UpdateTarget_Reachabilities(){
        
        foreach(Target target in targets){
            if (navMeshAgent.CalculatePath(target.gameObject.transform.position, path)){
                Debug.Log(target.name + ": " + target.isOneCurrentDestination);
                if (path.status == NavMeshPathStatus.PathComplete && !target.isOneCurrentDestination){
                    target.isReachable = true;
                }
                else if(target.isOneCurrentDestination){
                    target.isReachable = false;
                    target.isOneCurrentDestination = false;
                }
            }
        }
    }

    List<Target> GetReachableTargets(){

        List<Target> openTargets = new List<Target>(); 

        foreach(Target t in targets){
            if (t.isReachable) openTargets.Add(t);
        }

        return openTargets;
    }


    int RandomTargetIndex(List<Target> openTargets){
        int targetIndex = Random.Range(0, openTargets.Count);
        //Debug.Log(target_Index);
        return targetIndex;
    }
    void GenerateRandomSeed(){
        int tempSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(tempSeed);
    }

   
    //gets called when children exits Zone again
    public void ReOpenTarget(Collider target_collider){
        target_collider.gameObject.GetComponent<Target>().isReachable = true;
        target_collider.gameObject.GetComponent<Target>().isOneCurrentDestination = false;
    }
    
}
