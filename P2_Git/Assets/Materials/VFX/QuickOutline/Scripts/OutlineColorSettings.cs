using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "OutlineColorSettings", menuName = "ScriptableObjects/OutlineColorSettings")]
public class OutlineColorSettings : ScriptableObject
{
    public Color outlineColor = Color.white;

    public Color outlineColorpressed = Color.red;
    [SerializeField]
    public int onselectedwidth = 1;
    [SerializeField]
    public int defaultwidth = 1;
}
