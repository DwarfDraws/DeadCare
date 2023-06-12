using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    
    NavMesh navMesh;
    [SerializeField] GameObject childPrefab;


    public int childrenToSpawn;
    bool isBlocked;
    bool isSpawning;
   
    private void Start() {
        navMesh = GameObject.Find("NavMesh_Handler").GetComponent<NavMesh>();
    }

    private void Update() {

        if(isSpawning && !isBlocked && childrenToSpawn > 0)
        {
            GameObject newChild = GameObject.Instantiate(childPrefab, this.gameObject.transform.position, Quaternion.identity);
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
        GameObject newChild = GameObject.Instantiate(childPrefab, this.gameObject.transform.position, Quaternion.identity);
        NavMeshAgent newAgent = newChild.GetComponent<NavMeshAgent>();
        navMesh.Add_Agent(newAgent);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "child") isBlocked = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "child") isBlocked = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "child") isBlocked = false;
    }
}
