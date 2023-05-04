using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_attributes : MonoBehaviour
{
    public bool isRotated;  
    public bool isMoveable = true; //initially true, can be changed

    bool isClamped_left, isClamped_right, isClamped_front, isClamped_back;


    public bool IsClamped_Left(){
        return isClamped_left;
    } 
    public bool IsClamped_Right(){
        return isClamped_right;
    } 
    public bool IsClamped_Front(){
        return isClamped_front;
    } 
    public bool IsClamped_Back(){
        return isClamped_back;
    } 

    public void SetClamp_Left(bool isClamped){
        if(isClamped) isClamped_left = true;
        else isClamped_left = false;
    }
    public void SetClamp_Right(bool isClamped){
        if(isClamped) isClamped_right = true;
        else isClamped_right = false;

    }
    public void SetClamp_Front(bool isClamped){
        if(isClamped) isClamped_front = true;
        else isClamped_front = false;
    }
    public void SetClamp_Back(bool isClamped){
        if(isClamped) isClamped_back = true;
        else isClamped_back = false;
    }
}