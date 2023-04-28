using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    
    NavMesh navMesh;
    [SerializeField] GameObject childPrefab;
   
    private void Start() {
        navMesh = GameObject.Find("NavMesh_Handler").GetComponent<NavMesh>();
    }
    public void SpawnChild(){
        
        GameObject newChild = GameObject.Instantiate(childPrefab, this.gameObject.transform.position, Quaternion.identity);
        NavMeshAgent newAgent = newChild.GetComponent<NavMeshAgent>();
        navMesh.AddAgent(newAgent);
    }
}
