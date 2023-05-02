using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour
{
        
    [SerializeField] NavMesh navMesh;
    [SerializeField] LayerMask layer_Floor;
    [SerializeField] Camera main_camera;
    [SerializeField] Canvas_Script canvas_script;
    [SerializeField] Object_moveHandler object_MoveHandler;
    RaycastHit hit;
    Object_attributes obj_attributes;
    Ray ray;

    [SerializeField] Transform consumable_ghostObject;
    Transform hitObject;

    Vector3 mouse_Pos3D, offset, mouse3D_wOffset;
    Vector3 initMouse_Pos, initHit_Offset;

    bool MousePressed_L;
    bool missingOffset;
    bool isObstacle;


    float localLength_x_Object, localLength_z_Object;
    float obj_posY;


    void Update()
    {

        //Object Detection (Left Mouse-Click)
        if (Input.GetMouseButtonDown(0) && !MousePressed_L)
        {
            ray = main_camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, float.MaxValue, ~layer_Floor))
            { 
                hitObject = hit.transform; //object transform

                if(hitObject.tag == "obstacle"){
                    if(hitObject.GetComponent<Object_attributes>().isMoveable){
                        isObstacle = true;                    //EDIT THIS
                        obj_attributes = hitObject.GetComponent<Object_attributes>();
                        localLength_x_Object = hitObject.lossyScale.x;
                        localLength_z_Object = hitObject.lossyScale.z;
                        object_MoveHandler.SetObject(obj_attributes, localLength_x_Object, localLength_z_Object);

                        initMouse_Pos = hit.point; //mouse position
                        initMouse_Pos.y = 0;

                        MousePressed_L = true;
                        missingOffset = true;
                    }
                }
            }
            else isObstacle = false;  
        }

        if (Input.GetMouseButton(0))
        {
            //Object Transformation
            if(MousePressed_L && isObstacle){
                ray = main_camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, float.MaxValue, layer_Floor))
                {
                    mouse_Pos3D = hit.point;

                    if (missingOffset)
                    {
                        UpdateOffset();
                        UpdateInitOffset();
                        missingOffset = false;
                    }

                    offset.y = 0;
                    mouse_Pos3D.y = 0;

                    object_MoveHandler.CheckInput_Rotation(hitObject, mouse_Pos3D, initHit_Offset);

                    mouse3D_wOffset = mouse_Pos3D - offset;         
                    object_MoveHandler.ClampObject(mouse3D_wOffset, hitObject.position, mouse3D_wOffset);

                    //object translation
                    obj_posY = hitObject.position.y;   
                    float x = object_MoveHandler.GetClampedPosX_Object(mouse3D_wOffset);
                    float z = object_MoveHandler.GetClampedPosZ_Object(mouse3D_wOffset);   
                    hitObject.position = new Vector3(x, obj_posY, z);
                }
            }


        }
            
        //Left Mouse Up finally re-calculates NavMesh
        if (Input.GetMouseButtonUp(0))
        {
            if(isObstacle){
                MousePressed_L = false;
                missingOffset = false;

                navMesh.RecalculateAllPaths();
            }
        }


        //collectible
        if(canvas_script.isBtnPressed_Consumable()){
            ray = main_camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, float.MaxValue, layer_Floor)){
                mouse_Pos3D = hit.point;
                mouse_Pos3D.y = consumable_ghostObject.position.y;

                localLength_x_Object = consumable_ghostObject.localScale.x;
                localLength_z_Object = consumable_ghostObject.localScale.z;

                consumable_ghostObject.position = mouse_Pos3D;
            }
        }
    }

    public Vector3 GetMousePos3D(){
        return mouse_Pos3D;
    }

    public void UpdateOffset()
    {
        offset = mouse_Pos3D - hitObject.position;
    }
    
    void UpdateInitOffset()
    {
        initHit_Offset = initMouse_Pos - mouse_Pos3D;
    }
    
}

