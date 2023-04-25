using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour
{
    
    
    [SerializeField] NavMesh navMesh;
    
    public Camera camera;
    [SerializeField] LayerMask layer_Floor;
    RaycastHit hit;
    Ray ray;
    bool MousePressed_L, missingOffset;
    Transform hitObject;
    Vector3 mouse_Pos3D, offset;
    Vector3 initHit_Pos, initHit_Offset;

    public GameObject turnAroundAxis;


    private void Start() {

    }

    void Update(){

        if (Input.GetMouseButtonDown(0) && !MousePressed_L){       
            ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, float.MaxValue, ~layer_Floor)){
                hitObject = hit.transform; //object transform

                //Debug.Log(hitObject.position);
                initHit_Pos = hit.point; //mouse position
                initHit_Pos.y = 0;

                MousePressed_L = true;
                missingOffset = true;
            }
        }



        if (Input.GetMouseButton(0) && MousePressed_L){
            ray = camera.ScreenPointToRay(Input.mousePosition);
            
            if(Physics.Raycast(ray, out hit, float.MaxValue, layer_Floor)){
                mouse_Pos3D = hit.point;
                //Debug.Log(mouse_Pos3D);

                if(missingOffset){
                    UpdateOffset();
                    UpdateInitOffset();
                    missingOffset = false;
                }
                //Debug.Log(offset + " = Offset");

                offset.y = 0;
                mouse_Pos3D.y = 0;

                if(Input.GetKeyDown("q") || Input.GetKeyDown("e")){
                    var key = Input.inputString;
                    float rotateAngle = 0;
                    switch (key) 
                    {
                        case "q":   rotateAngle = -90.0f;    break;
                        case "e":   rotateAngle = 90.0f;   break;
                        default: break;
                    }
                    hitObject.RotateAround(mouse_Pos3D + initHit_Offset, Vector3.up, rotateAngle);
                    UpdateOffset();
                }
                else hitObject.position = mouse_Pos3D - offset;

            }
        }


        if (Input.GetMouseButtonUp(0)){
            MousePressed_L = false;
            missingOffset = false;
            navMesh.RecalculatePath();
        }
    }

    void UpdateOffset(){
        offset = mouse_Pos3D - hitObject.position;
    }
    
    void UpdateInitOffset(){
        initHit_Offset = initHit_Pos - mouse_Pos3D;
    }
}

