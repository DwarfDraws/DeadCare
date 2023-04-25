using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInput : MonoBehaviour
{
    public GameObject testObject;
    float movingSpeed = 0.5f;
    Vector3 translationVector;
    
    // Start is called before the first frame update
    void Start()
    {
	testObject = GameObject.Find("Obstacles");
	translationVector = Vector3.left * movingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetKey("a")){
		testObject.transform.position += translationVector;		
	}    
    }

}
