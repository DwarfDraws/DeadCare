using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    
    NavMesh navMesh;
    [SerializeField] GameObject[] childPrefabs;

    [HideInInspector] public int childrenToSpawn;
    [HideInInspector] public float children_walkSpeed = 1.25f;
    bool isBlocked;
    bool isSpawning;
   
    string navMeshHandler_name = "NavMesh_Handler";
    string tag_child = "child";

    private void Start() 
    {
        navMesh = GameObject.Find(navMeshHandler_name).GetComponent<NavMesh>();
    }

    private void Update() 
    {
        if(isSpawning && !isBlocked && childrenToSpawn > 0)
        {
            int rndmIndex = Random.Range(0, 2);

            GameObject newChild = GameObject.Instantiate(childPrefabs[rndmIndex], this.gameObject.transform.position, Quaternion.identity);
            NavMeshAgent newAgent = newChild.GetComponent<NavMeshAgent>();
            navMesh.Add_Agent(newAgent);

            childrenToSpawn--;
            if(childrenToSpawn == 0) isSpawning = false;

            isBlocked = true;
        }
    }

    public void SpawnChildren()
    {
        isBlocked = false;
        isSpawning = true;
    }

    //single spawn by UI Button
    public void SpawnChild()
    {
        GameObject newChild = GameObject.Instantiate(childPrefabs[0], this.gameObject.transform.position, Quaternion.identity);
        NavMeshAgent newAgent = newChild.GetComponent<NavMeshAgent>();
        newAgent.speed = children_walkSpeed;
        
        navMesh.Add_Agent(newAgent);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == tag_child) isBlocked = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == tag_child) isBlocked = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == tag_child) isBlocked = false;
    }
}
