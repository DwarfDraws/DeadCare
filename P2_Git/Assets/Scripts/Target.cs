using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool isDeadly;
    public bool isReachable; //true, if a child selected this target
    public bool isOneCurrentDestination;
    public float waitTime_seconds = 5.0f; //initially 5 secs, can be changed in the inspector
    

    float r, g, b;

    [SerializeField] Renderer renderer;
    Material mat;
    

    /*
    void Start()
    {
        

        r = g = b = 0.0f;
        mat = this.gameObject.GetComponent<Renderer>().material;
        mat.color = new Color(r,g,b);
    }

    void Update()
    {
        r++;
        g++;
        b++;        
    }
    */
}
