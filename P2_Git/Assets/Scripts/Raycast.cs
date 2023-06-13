using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour
{
        
    [SerializeField] NavMesh navMesh;
    [SerializeField] Object_moveHandler object_MoveHandler;
    [SerializeField] Menu_Handler menu_Handler;
    Canvas_Script canvas;
    Gameplay gameplay;
    Object_attributes obj_attributes;
    Target object_attachedTarget;
    Animation_Script object_Animation;
    
    [SerializeField] Camera main_camera;
    [SerializeField] LayerMask layer_Floor;
    RaycastHit hit;
    Ray ray;

    [SerializeField] Transform consumable_ghostObject;
    Transform hitObject;
    Vector3 mouse_Pos3D, offset, mouse3D_wOffset;
    Vector3 initMouse_Pos, initHit_Offset;

    public bool isMoveable;
    bool MousePressed_L;
    bool missingOffset;
    bool hasAttachedTarget;
    bool isObject_Animation_Rewinded;
    float localLength_x_Object, localLength_z_Object;
    float obj_posY;
    
    string canvas_name = "InGameUI";


    //Hakon
    private GameObject halochild;

    private void Start() {
        gameplay = GameObject.Find("Gameplay_Handler").GetComponent<Gameplay>();
        canvas = GameObject.Find(canvas_name).GetComponent<Canvas_Script>();
    }

    void Update()
    {
   
        if(!menu_Handler.isGameOver)
        {
            //Object Detection (Left Mouse-Click)
            if (Input.GetMouseButtonDown(0) && !MousePressed_L)
            {
                ray = main_camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, float.MaxValue, ~layer_Floor))
                { 
                    hitObject = hit.transform; //object transform

                    if(hitObject.CompareTag("obstacle"))
                    {
                        //move
                        if (canvas.isMoveBtnPressed && hitObject.GetComponent<Object_attributes>().isMoveable)
                        {
                            isMoveable = true;                  
                            MousePressed_L = true;

                            obj_attributes = hitObject.GetComponent<Object_attributes>();
                            localLength_x_Object = hitObject.lossyScale.x;
                            localLength_z_Object = hitObject.lossyScale.z;
                            object_MoveHandler.SetObject(obj_attributes, localLength_x_Object, localLength_z_Object);

                            initMouse_Pos = hit.point; //mouse position
                            initMouse_Pos.y = 0;

                            missingOffset = true;
                        }
                        else isMoveable = false;

                        //get attached target
                        if (hit.transform.GetComponent<Object_attributes>().attachedTarget != null)
                        {
                            hasAttachedTarget = true;
                            MousePressed_L = true;

                            object_attachedTarget = hit.transform.GetComponent<Object_attributes>().attachedTarget;
                            object_Animation = hit.transform.GetComponent<Animation_Script>();
                        }
                        else hasAttachedTarget = false;

                        //tape
                        if(canvas.isTapeBtnPressed && !hitObject.GetComponent<Object_attributes>().isMoveable)
                        {   

                            ////////////////////////
                            //turnon halo -> by Hakon & Anastasia
                            if(hitObject.transform.Find("halo").gameObject)
                            { 
                                halochild = hitObject.transform.Find("halo").gameObject;

                                if (halochild.CompareTag("tapeHalo"))
                                {
                                    halochild.SetActive(true);
                                }
                            }
                            ////////////////////////////////


                            obj_attributes = hitObject.GetComponent<Object_attributes>();
                            if (obj_attributes.isTaped)
                            {
                                if(obj_attributes.attachedTarget != null){
                                    obj_attributes.attachedTarget.SetTargetUntaped(); //!!!!!!!! Should not be here, only on Obj itselfe
                                    obj_attributes.SetTapeActive(false);
                                } 

                                gameplay.IncreaseTapeCount();
                            }
                            else if (gameplay.GetTapeCount() > 0) 
                            {
                                if(obj_attributes.attachedTarget != null){ 
                                    obj_attributes.attachedTarget.SetTargetTaped(); //!!!!!!!! same problem
                                    obj_attributes.SetTapeActive(true);

                                    gameplay.DecreaseTapeCount();
                                } 

                            }

                        }
                    }
                    else 
                    {
                        MousePressed_L = false;
                    }
                }
            }


            if (Input.GetMouseButton(0))
            {
                //Object Transformation
                if(MousePressed_L)
                {
                    //switch timer to go up 
                    if(hasAttachedTarget && !canvas.isTapeBtnPressed && !canvas.isMoveBtnPressed)
                    { 
                        object_attachedTarget.ToggleDown(false);
                        if(!isObject_Animation_Rewinded) 
                        {   
                            if(object_Animation != null) object_Animation.RewindAnimation();
                            isObject_Animation_Rewinded = true;
                        }

                    }


                    if(isMoveable && canvas.isMoveBtnPressed)
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

                            object_MoveHandler.CheckInput_Rotation(hitObject, mouse_Pos3D, initHit_Offset);

                            mouse3D_wOffset = mouse_Pos3D - offset;         
                            object_MoveHandler.ClampObject(mouse3D_wOffset, hitObject.position, mouse3D_wOffset);

                            //object translation
                            //EDIT: Move to object_Movehandler
                            obj_posY = hitObject.position.y;   
                            float x = object_MoveHandler.GetClampedPosX_Object(mouse3D_wOffset);
                            float z = object_MoveHandler.GetClampedPosZ_Object(mouse3D_wOffset);   
                            hitObject.position = new Vector3(x, obj_posY, z); 
                        }
                    }

                }
            }
        }
            
        //Left Mouse Up finally re-calculates NavMesh
        //EDIT weist ja
        if (Input.GetMouseButtonUp(0))
        {
            //////////////////////////////////
            //Hakon
            if (halochild != null)
            {
                halochild.SetActive(false);
            }
            /////////////////////////////////
            /// 
            if (MousePressed_L){
                MousePressed_L = false;
                missingOffset = false;

                //switch timer to go down 
                if(hasAttachedTarget)
                {
                    object_attachedTarget.ToggleDown(true);
                    if(isObject_Animation_Rewinded) 
                    {
                        if(object_Animation != null) object_Animation.RewindAnimation();
                        isObject_Animation_Rewinded = false;
                    }

                }

                //navMesh.RecalculateAllPaths();
            }
        }


        //consumables
        if(canvas.isBtnPressed_Consumable()){
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

