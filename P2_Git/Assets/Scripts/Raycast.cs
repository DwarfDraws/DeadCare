using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour
{
    Object_attributes object_info;
        
    [SerializeField] NavMesh navMesh;
    [SerializeField] GameObject floor;
    [SerializeField] LayerMask layer_Floor;
    [SerializeField] Camera main_camera;

    RaycastHit hit;
    Ray ray;

    Transform hitObject;

    Vector3 mouse_Pos3D, offset;
    Vector3 initHit_Pos, initHit_Offset;

    bool MousePressed_L;
    bool missingOffset;
    bool moveableObject;
    bool isClamped_left, isClamped_right, isClamped_front, isClamped_back;

    float hitObject_localLength_x, hitObject_localLength_z;
    float floor_length_x, floor_depth_z; 

    float distanceToGap_x, distanceToGap_z;
    float mouseClampPos_x, mouseClampPos_z;

    private void Start() {
        
        floor_length_x = floor.transform.localScale.x;
        floor_depth_z = floor.transform.localScale.z;

    }
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
                        moveableObject = true;                    //EDIT THIS
                        object_info = hitObject.GetComponent<Object_attributes>();
                        hitObject_localLength_x = hitObject.lossyScale.x;
                        hitObject_localLength_z = hitObject.lossyScale.z;

                        initHit_Pos = hit.point; //mouse position
                        initHit_Pos.y = 0;

                        MousePressed_L = true;
                        missingOffset = true;
                    }
                    else moveableObject = false;
                }
            }
        }

        if (moveableObject)
        { 
            //Object Transformation (Left Mouse pressed)
            if (Input.GetMouseButton(0) && MousePressed_L)
            {
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

                    //rotation
                    if (Input.GetKeyDown("q") || Input.GetKeyDown("e"))
                    {
                        var key = Input.inputString;
                        float rotateAngle = 0;
                        switch (key)
                        {
                            case "q": rotateAngle = -90.0f; break;
                            case "e": rotateAngle = 90.0f; break;
                            default: break;
                        }
                        hitObject.RotateAround(mouse_Pos3D + initHit_Offset, Vector3.up, rotateAngle);
                        object_info.isRotated = !object_info.isRotated;
                        UpdateOffset();
                    }


                    //check boundaries for translation, clamp 
            
                    Vector3 mouse3D = mouse_Pos3D - offset;
                    float obj_posY = hitObject.position.y;
                    float half_floorLength_x = 0.5f * floor_length_x;
                    float half_floorDepth_z = 0.5f * floor_depth_z;
                    
                    //X-Axis 
                    //left (-x)
                    if (!isClamped_right)
                    {
                        //Debug.Log(DistanceToGap_X(true));
                        if (DistanceToGap_X(true) <= 0)
                        {
                            
                            mouseClampPos_x = mouse3D.x; //save mousePos at clamp-time
                            isClamped_right = true;
                        }

                    }
                    //right (+x)
                    if (!isClamped_left)
                    {
                        if (DistanceToGap_X(false) >= floor_length_x)
                        {
                            mouseClampPos_x = mouse3D.x;
                            isClamped_left = true;
                        }
                    }

                    //Z-Axis
                    //front (-z)
                    if (!isClamped_front)
                    {
                        //Debug.Log(DistanceToGap_Z(true));
                        if (DistanceToGap_Z(true) <= 0)
                        {
                            mouseClampPos_z = mouse3D.z;
                            isClamped_front = true;
                        }
                    }
                    //back (+z)
                    if (!isClamped_back)
                    {

                        if (DistanceToGap_Z(false) >= floor_depth_z)
                        {
                            mouseClampPos_z = mouse3D.z;
                            isClamped_back = true;
                        }
                    }


                    //object translation
                    float clamped_X, clamped_Z;

                    if(isClamped_left)          clamped_X = -half_floorLength_x + Object_OffsetToOuterBorder_X(); 
                    else if(isClamped_right)    clamped_X = half_floorLength_x - Object_OffsetToOuterBorder_X();
                    else                        clamped_X = mouse3D.x;
                    if(isClamped_back)          clamped_Z = -half_floorDepth_z + Object_OffsetToOuterBorder_Z();
                    else if(isClamped_front)    clamped_Z = half_floorDepth_z - Object_OffsetToOuterBorder_Z();
                    else                        clamped_Z = mouse3D.z;
                    
                    hitObject.position = new Vector3(clamped_X, obj_posY, clamped_Z);


                    //un-clamp & object translation
                    if (isClamped_right && mouseClampPos_x > mouse3D.x)
                    {
                        isClamped_right = false;
                        hitObject.position = mouse_Pos3D - offset; //move away in same frame to un-clamp 
                    }

                    else if (isClamped_left && mouseClampPos_x < mouse3D.x)
                    {
                        isClamped_left = false;
                        hitObject.position = mouse_Pos3D - offset;
                    }

                    else if (isClamped_front && mouseClampPos_z > mouse3D.z)
                    {
                        isClamped_front = false;
                        hitObject.position = mouse_Pos3D - offset;
                    }

                    else if (isClamped_back && mouseClampPos_z < mouse3D.z)
                    {
                        isClamped_back = false;
                        hitObject.position = mouse_Pos3D - offset;
                    }
                }
            }
            
            //Left Mouse Up finally re-calculates NavMesh
            if (Input.GetMouseButtonUp(0))
            {
                MousePressed_L = false;
                missingOffset = false;

                navMesh.Update_AllTargeted();
                navMesh.RecalculateAllPaths();
            }
        }
    }

    float DistanceToGap_X(bool isLeftMove)
    {
        float obj_posX = hitObject.position.x;
        float floor_halfLength_x = 0.5f * floor_length_x;

        distanceToGap_x = floor_halfLength_x - Object_borderPosX(isLeftMove, obj_posX);

        return distanceToGap_x;
    }

    float Object_borderPosX(bool isRightMove, float obj_posX)
    {
        float hitObject_border_x;
         
        if (isRightMove)    hitObject_border_x = obj_posX + Object_OffsetToOuterBorder_X();
        else                hitObject_border_x = obj_posX - Object_OffsetToOuterBorder_X();

        return hitObject_border_x;
    }

    float Object_OffsetToOuterBorder_X()
    {
        float offset_toOuterBorder;
        float outerBound_x;
        
        if (!object_info.isRotated) outerBound_x = hitObject_localLength_x;
        else                        outerBound_x = hitObject_localLength_z;

        offset_toOuterBorder = 0.5f * outerBound_x;
        return offset_toOuterBorder;
    }


    float DistanceToGap_Z(bool isForwardMove)
    {
        float obj_posZ = hitObject.position.z;
        float floor_halfLength_z = 0.5f * floor_depth_z;
        
        distanceToGap_z = floor_halfLength_z - Object_borderPosZ(isForwardMove, obj_posZ);

        return distanceToGap_z;      
    }

    float Object_borderPosZ(bool isBackwardMove, float obj_posZ)
    {
        float hitObject_border_z;
         
        if (isBackwardMove)     hitObject_border_z = obj_posZ + Object_OffsetToOuterBorder_Z();
        else                    hitObject_border_z = obj_posZ - Object_OffsetToOuterBorder_Z();

        return hitObject_border_z;
    }

    float Object_OffsetToOuterBorder_Z()
    {
        float offset_toOuterBorder;
        float outerBound_z;
        
        if (!object_info.isRotated) outerBound_z = hitObject_localLength_z;
        else                        outerBound_z = hitObject_localLength_x;

        offset_toOuterBorder = 0.5f * outerBound_z;
        return offset_toOuterBorder;
    }


    void UpdateOffset()
    {
        offset = mouse_Pos3D - hitObject.position;
    }
    
    void UpdateInitOffset()
    {
        initHit_Offset = initHit_Pos - mouse_Pos3D;
    }
}

