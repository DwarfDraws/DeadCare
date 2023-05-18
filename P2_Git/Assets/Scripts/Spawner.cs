using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    
    NavMesh navMesh;
    [SerializeField] GameObject childPrefab;

    bool isBlocked;
   
    private void Start() {
        navMesh = GameObject.Find("NavMesh_Handler").GetComponent<NavMesh>();
    }
    public void SpawnChild(){

        if (!isBlocked)
        {
            GameObject newChild = GameObject.Instantiate(childPrefab, this.gameObject.transform.position, Quaternion.identity);
            NavMeshAgent newAgent = newChild.GetComponent<NavMeshAgent>();
            navMesh.Add_Agent(newAgent);
        }
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
