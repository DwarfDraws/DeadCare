using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_attributes : MonoBehaviour
{
    public Target attachedTarget;
    [SerializeField] GameObject tape;
    public GameObject moveableHalo, tapeableHalo;

    Material moveableHalo_Mat;
    Color init_moveableHalo_Color;

    [HideInInspector] public bool isRotated; 
    [HideInInspector] public bool isTaped; 
    [HideInInspector] public bool isInNoMoveArea;
    public bool isMoveable = true; 
    bool isClamped_left, isClamped_right, isClamped_front, isClamped_back;

    string tag_noMoveArea = "noMoveArea";

    void Start()
    {
        if(moveableHalo != null)
        {
            moveableHalo_Mat = moveableHalo.GetComponent<Renderer>().material;
            init_moveableHalo_Color = moveableHalo_Mat.GetColor("_BaseColor");
            
        }
    }

    public void SetTapeActive(bool isActive)
    {
        if(tape != null){
            if (isActive){
                isTaped = true;
                tape.SetActive(true);
                attachedTarget.SetTargetTaped();
            }
            else{
                isTaped = false;
                tape.SetActive(false);
                attachedTarget.SetTargetUntaped();
            }
        }
    }


    public bool IsClamped_Left(){ return isClamped_left; } 
    public bool IsClamped_Right(){ return isClamped_right; } 
    public bool IsClamped_Front(){ return isClamped_front; } 
    public bool IsClamped_Back(){ return isClamped_back; } 

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


    private void OnTriggerStay(Collider other)
    {
        if(isMoveable && other.tag == tag_noMoveArea && !isInNoMoveArea)
        {
            isInNoMoveArea = true;
            if(moveableHalo_Mat != null) moveableHalo_Mat.SetColor("_BaseColor", Color.red);
        }    
    }
    private void OnTriggerExit(Collider other) 
    {
        if(isMoveable && other.tag == tag_noMoveArea)
        {
            isInNoMoveArea = false;
            moveableHalo_Mat.SetColor("_BaseColor", init_moveableHalo_Color);
        }    
    }

}
