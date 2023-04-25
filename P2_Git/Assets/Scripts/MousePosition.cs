using UnityEngine;
using System.Collections;

public class MousePosition : MonoBehaviour
{
    GameObject testObject;
    GameObject floor;
    float movingSpeed = 0.005f;
    Vector3 translationVector;

    float testObject_localLength_x;
    float floor_length_x = 10.0f;	//hard-coded!!
	
    float distanceToGap_x;

    void Start()
    {
	testObject = GameObject.Find("Obstacle1");
	testObject_localLength_x = testObject.transform.lossyScale.x;

	floor = GameObject.Find("Floor");

	translationVector = Vector3.left * movingSpeed;
    }
	

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            {
                //Debug.Log(mousePos.x);
                //Debug.Log(mousePos.y);
            }
        }
		 	

    	if(Input.GetKey("a") && DistanceToGap_X(true) <= floor_length_x){
		testObject.transform.position += translationVector;		
	}    
	else if (Input.GetKey("d") && DistanceToGap_X(false) >= 0){
		testObject.transform.position -= translationVector;		
	}
    }

    float DistanceToGap_X(bool isLeftMove){
	float floor_halfLength_x = 0.5f * floor_length_x;
	float testObject_border_x;
	float testObject_posX = testObject.transform.position.x;

	if(isLeftMove) 	testObject_border_x = testObject_posX - 0.5f * testObject_localLength_x;
	else 		testObject_border_x = testObject_posX + 0.5f * testObject_localLength_x;
	
	distanceToGap_x = floor_halfLength_x - testObject_border_x;
   	return distanceToGap_x;
    }
}