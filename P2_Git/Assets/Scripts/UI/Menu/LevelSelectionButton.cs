using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class LevelSelectionButton : MonoBehaviour
{
   
    public event System.Action<int> OnButtonClicked;
    private int _keyNumber;

    /*private void OnValidate()
    {
        _keyNumber = 3;//transform.GetSiblingIndex() + 1;//

    }*/
    private void Awake()
    {
        _keyNumber = transform.GetSiblingIndex() + 1;
        GetComponent<Button>().onClick.AddListener(HandleClick); 
       
    }

    private void HandleClick()
    {
        OnButtonClicked?.Invoke(_keyNumber);
    }

   
}
