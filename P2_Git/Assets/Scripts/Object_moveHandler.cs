using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_moveHandler : MonoBehaviour
{

    [SerializeField] Raycast raycast;
    Object_attributes obj_attributes;
    
    [SerializeField] GameObject floor;

    float mouseClampPos_x, mouseClampPos_z;
    float distanceToGap_x, distanceToGap_z;
    float floor_length_x, floor_depth_z; 
    float half_floorLength_x, half_floorDepth_z;
    float localLength_x_Object, localLength_z_Object;


    private void Start() {
        floor_length_x  = floor.transform.localScale.x;
        floor_depth_z   = floor.transform.localScale.z;

        half_floorLength_x  = 0.5f * floor_length_x;
        half_floorDepth_z   = 0.5f * floor_depth_z;
    }

    public void CheckInput_Rotation(Transform hitObject, Vector3 mouse_Pos3D, Vector3 initHit_Offset)
    {     
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            var key = Input.inputString;
            float rotateAngle = 90f;

            hitObject.RotateAround(mouse_Pos3D + initHit_Offset, Vector3.up, rotateAngle);
            obj_attributes.isRotated = !obj_attributes.isRotated;

            raycast.UpdateOffset();
        }
    }

    public void ClampObject(Vector3 mousePos, Vector3 obj_pos, Vector3 mouse3D_wOffset)
    {
        //X-Axis 
        if (!obj_attributes.IsClamped_Right() && IsObjectAtEdge_Right(obj_pos))
        {                       
            mouseClampPos_x = mousePos.x; //save mousePos at clamp-time
            obj_attributes.SetClamp_Right(true);
        }

        if (!obj_attributes.IsClamped_Left() && IsObjectAtEdge_Left(obj_pos))
        {
            mouseClampPos_x = mousePos.x;
            obj_attributes.SetClamp_Left(true);
        }

        //Z-Axis
        if (!obj_attributes.IsClamped_Back() && IsObjectAtEdge_Back(obj_pos))
        {
            mouseClampPos_z = mousePos.z;
            obj_attributes.SetClamp_Back(true);
        }

        if (!obj_attributes.IsClamped_Front() && IsObjectAtEdge_Front(obj_pos))
        {
            mouseClampPos_z = mousePos.z;
            obj_attributes.SetClamp_Front(true);
        }

        //UN-CLAMP
        if      (obj_attributes.IsClamped_Right()  &&  mouseClampPos_x > mouse3D_wOffset.x)    obj_attributes.SetClamp_Right(false); 

        else if (obj_attributes.IsClamped_Left()   &&  mouseClampPos_x < mouse3D_wOffset.x)    obj_attributes.SetClamp_Left(false);

        else if (obj_attributes.IsClamped_Back()   &&  mouseClampPos_z < mouse3D_wOffset.z)    obj_attributes.SetClamp_Back(false);           

        else if (obj_attributes.IsClamped_Front()  &&  mouseClampPos_z > mouse3D_wOffset.z)    obj_attributes.SetClamp_Front(false);
    }


    public bool IsObjectAtEdge(Vector3 obj_transform){
        if(IsObjectAtEdge_Left(obj_transform) || IsObjectAtEdge_Right(obj_transform) || 
            IsObjectAtEdge_Front(obj_transform)|| IsObjectAtEdge_Back(obj_transform))
        { 
                return true;
        }
        
        else return false;
    }
    public bool IsObjectAtEdge_Left(Vector3 obj_transform){ 
        if(DistanceToGap_X(false, obj_transform) >= floor_length_x)     return true;
        else return false;
    }
    public bool IsObjectAtEdge_Right(Vector3 obj_transform){ 
        if(DistanceToGap_X(true, obj_transform) <= 0)                   return true;
        else return false;
    }
    public bool IsObjectAtEdge_Front(Vector3 obj_transform){ 
        if(DistanceToGap_Z(true, obj_transform) <= 0)                   return true;
        else return false;
    }
    public bool IsObjectAtEdge_Back(Vector3 obj_transform){ 
        if(DistanceToGap_Z(false, obj_transform) >= floor_depth_z)      return true;
        else return false;
    }


    float DistanceToGap_X(bool isLeftMove, Vector3 obj_pos)
    {
        float floor_halfLength_x = 0.5f * floor_length_x;

        distanceToGap_x = floor_halfLength_x - Object_borderPosX(isLeftMove, obj_pos.x);

        return distanceToGap_x;
    }
    float DistanceToGap_Z(bool isForwardMove, Vector3 obj_pos)
    {
        float obj_posZ = obj_pos.z;
        float floor_halfLength_z = 0.5f * floor_depth_z;
        
        distanceToGap_z = floor_halfLength_z - Object_borderPosZ(isForwardMove, obj_posZ);

        return distanceToGap_z;      
    }


    float Object_borderPosX(bool isRightMove, float obj_posX)
    {
        float hitObject_border_x;
         
        if (isRightMove)    hitObject_border_x = obj_posX + Object_OffsetToOuterBorder_X();
        else                hitObject_border_x = obj_posX - Object_OffsetToOuterBorder_X();

        return hitObject_border_x;
    }
    float Object_borderPosZ(bool isBackwardMove, float obj_posZ)
    {       
        float hitObject_border_z;
         
        if (isBackwardMove)     hitObject_border_z = obj_posZ + Object_OffsetToOuterBorder_Z();
        else                    hitObject_border_z = obj_posZ - Object_OffsetToOuterBorder_Z();

        return hitObject_border_z;
    }


    public float GetClampedPosX_Object(Vector3 mouse3D_wOffset){       
        float x;
        
        if(obj_attributes.IsClamped_Left())            x = -half_floorLength_x + Object_OffsetToOuterBorder_X(); 
        else if(obj_attributes.IsClamped_Right())      x = half_floorLength_x - Object_OffsetToOuterBorder_X();
        else                                x = mouse3D_wOffset.x;

        return x;
    }
    public float GetClampedPosZ_Object(Vector3 mouse3D_wOffset){     
        float z;
        
        if(obj_attributes.IsClamped_Front())           z = half_floorDepth_z - Object_OffsetToOuterBorder_Z();
        else if(obj_attributes.IsClamped_Back())       z = -half_floorDepth_z + Object_OffsetToOuterBorder_Z();
        else                                z = mouse3D_wOffset.z;        

        return z;
    }


    float Object_OffsetToOuterBorder_X()
    {
        
        float offset_toOuterBorder;
        float outerBound_x;
        
        if (!obj_attributes.isRotated)  outerBound_x = localLength_x_Object;
        else                            outerBound_x = localLength_z_Object;

        offset_toOuterBorder = 0.5f * outerBound_x;
        return offset_toOuterBorder;
    }
    float Object_OffsetToOuterBorder_Z()
    {
        
        float offset_toOuterBorder;
        float outerBound_z;
        
        if (!obj_attributes.isRotated)  outerBound_z = localLength_z_Object;
        else                            outerBound_z = localLength_x_Object;

        offset_toOuterBorder = 0.5f * outerBound_z;
        return offset_toOuterBorder;
    }

    public void SetObject(Object_attributes obj_attributes_, float localLength_x_Object_, float localLength_z_Object_){
        obj_attributes = obj_attributes_;
        localLength_x_Object = localLength_x_Object_;
        localLength_z_Object = localLength_z_Object_;
    }

}
