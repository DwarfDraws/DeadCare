using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour
{
    Object_info object_info;
        
    [SerializeField] NavMesh navMesh;
    [SerializeField] GameObject floor;
    [SerializeField] LayerMask layer_Floor;
    [SerializeField] Camera camera;

    RaycastHit hit;
    Ray ray;

    Transform hitObject;

    Vector3 mouse_Pos3D, offset;
    Vector3 initHit_Pos, initHit_Offset;

    bool MousePressed_L;
    bool missingOffset;
    bool moveableObject;
    bool isClamped_right, isClamped_left, isClamped_front, isClamped_back;

    float object_localLength_x, object_localLength_z;
    float floor_length_x = 10.0f, floor_depth_z = 10.0f; 

    float distanceToGap_x, distanceToGap_z;
    float mouseClamp_x, mouseClamp_z;


    void Update()
    {

        //Object Detection (Left Mouse-Click)
        if (Input.GetMouseButtonDown(0) && !MousePressed_L)
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, float.MaxValue, ~layer_Floor))
            {
                hitObject = hit.transform; //object transform
                object_info = hitObject.GetComponent<Object_info>();
                object_localLength_x = hitObject.lossyScale.x;
                object_localLength_z = hitObject.lossyScale.z;

                initHit_Pos = hit.point; //mouse position
                initHit_Pos.y = 0;

                MousePressed_L = true;
                missingOffset = true;
            }
        }

        if (object_info.isMoveable)
        { 
            //Object Transformation (Left Mouse pressed)
            if (Input.GetMouseButton(0) && MousePressed_L)
            {
                ray = camera.ScreenPointToRay(Input.mousePosition);

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


                    //check boundaries for translation
                    //X-Axis 
                    //left (-x)
                    if (!isClamped_left)
                    {

                        if (DistanceToGap_X(true) >= floor_length_x)
                        {
                            mouseClamp_x = (mouse_Pos3D - offset).x;
                            isClamped_left = true;
                        }
                    }
                    //right (+x)
                    if (!isClamped_right)
                    {
                        if (DistanceToGap_X(false) <= 0)
                        {
                            mouseClamp_x = (mouse_Pos3D - offset).x;
                            isClamped_right = true;
                        }
                    }

                    //Z-Axis
                    //front (+z)
                    if (!isClamped_front)
                    {
                        Debug.Log(DistanceToGap_Z(true));
                        if (DistanceToGap_Z(true) <= 0)
                        {
                            mouseClamp_z = (mouse_Pos3D - offset).z;
                            isClamped_front = true;
                        }
                    }
                    //back (-z)
                    if (!isClamped_back)
                    {

                        if (DistanceToGap_Z(false) >= floor_depth_z)
                        {
                            mouseClamp_z = (mouse_Pos3D - offset).z;
                            isClamped_back = true;
                        }
                    }

                    //object translation and un-clamp
                    if (!isClamped_left && !isClamped_right && !isClamped_front && !isClamped_back)
                        hitObject.position = mouse_Pos3D - offset;

                    else if (isClamped_left && mouseClamp_x < (mouse_Pos3D - offset).x)
                    {
                        isClamped_left = false;
                        hitObject.position = mouse_Pos3D - offset;
                    }

                    else if (isClamped_right && mouseClamp_x > (mouse_Pos3D - offset).x)
                    {
                        isClamped_right = false;
                        hitObject.position = mouse_Pos3D - offset;
                    }

                    else if (isClamped_front && mouseClamp_z > (mouse_Pos3D - offset).z)
                    {
                        isClamped_front = false;
                        hitObject.position = mouse_Pos3D - offset;
                    }

                    else if (isClamped_back && mouseClamp_z < (mouse_Pos3D - offset).z)
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
                navMesh.RecalculatePath();
            }
        }
    }

    void UpdateOffset(){
        offset = mouse_Pos3D - hitObject.position;
    }
    
    void UpdateInitOffset(){
        initHit_Offset = initHit_Pos - mouse_Pos3D;
    }


    float DistanceToGap_X(bool isRightMove)
    {
        float object_posX = hitObject.position.x;
        float floor_halfLength_x = 0.5f * floor_length_x;
        float object_border_x;
        float outerBound_x;

        if (!object_info.isRotated) outerBound_x = object_localLength_x;
        else                        outerBound_x = object_localLength_z;

        if (isRightMove)    object_border_x = object_posX - 0.5f * outerBound_x;
        else                object_border_x = object_posX + 0.5f * outerBound_x;

        distanceToGap_x = floor_halfLength_x - object_border_x;

        return distanceToGap_x;
    }


    float DistanceToGap_Z(bool isForwardMove)
    {
        float object_posZ = hitObject.position.z;
        float floor_halfLength_z = 0.5f * floor_depth_z;
        float object_border_z;
        float outerBound_z;

        if (!object_info.isRotated) outerBound_z = object_localLength_z;
        else                        outerBound_z = object_localLength_x;

        if (isForwardMove)  object_border_z = object_posZ + 0.5f * outerBound_z;
        else                object_border_z = object_posZ - 0.5f * outerBound_z;

        distanceToGap_z = floor_halfLength_z - object_border_z;
        
        return distanceToGap_z;
    }
}

