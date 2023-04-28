using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float waitTime_seconds = 2.0f; //initially 2 secs, can be changed in the inspector   
    public bool isDeadly;
    public bool isOpen; //true, if a child selected this target
    public bool isTargeted;
}
