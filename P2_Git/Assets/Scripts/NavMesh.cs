using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] NavMeshObstacle obstacle;
    public NavMeshAgent navMeshAgent;
    

    NavMeshPath path;

    bool pathCheckedTwice;
    float timer;


    private void Start() {
        path = new NavMeshPath();
        pathCheckedTwice = false;  
        timer = 0;
    }

    private void Update() {
        

        //VERY BAD CODE! Waiting for dynamic obstacles to finish calculating before checking Path 
        timer += Time.deltaTime;
        if(timer >= 0.05 && !pathCheckedTwice){
            RecalculatePath();
            pathCheckedTwice = true;
        }

    }

    //gets called when mousebutton is released after dragging dynamic obstacle
    public void RecalculatePath(){
        
        if (navMeshAgent.CalculatePath(target.position, path)) 
        {            
            if (path.status == NavMeshPathStatus.PathComplete){
                navMeshAgent.destination = target.position;
                navMeshAgent.isStopped = false;
            } 
            else{
                navMeshAgent.isStopped = true;

                
            } 
        }
    }
}
