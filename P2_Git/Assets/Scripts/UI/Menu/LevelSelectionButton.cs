using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class LevelSelectionButton : MonoBehaviour
{
    public event System.Action<int> OnButtonClicked;
    private KeyCode _keyCode;
    private int _keyNumber;

    private void OnValidate()
    {
        _keyNumber = transform.GetSiblingIndex() + 1;
        _keyCode = KeyCode.Alpha0 + _keyNumber;

    }
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }
    private void HandleClick()
    {
        OnButtonClicked?.Invoke(_keyNumber);
    }
}
