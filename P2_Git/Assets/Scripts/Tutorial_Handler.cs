using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Handler : MonoBehaviour
{
    [SerializeField] Canvas_Script canvas;
    [SerializeField] InGameUI _inGameUi;
    public GameObject _tutorialInfo;
    public GameObject _inGameMenuObject;
    public GameObject _countdownTimer, _btnFastForward, _btnTape, _btnMove;
    public AudioSource _audio;
    [SerializeField] TMP_Text _txtTutorialInfo;

    int _index;
    bool _active;
    string[] _currentInfo;
    string[] _txtInfo_1 =
    {
        "Welcome to DeadCare! Listen closely, we don't have much time until the kids arrive.",
        "You are in the preperation phase. You have this much time to prepare the room.",
        "Find potentially dangerous objects in this room. You can tape them, or move other objects in front of them so they can not be reached any more."
    };
    string[] _txtInfo_2 =
    {
        "The children have arrived.", 
        "A new timer has started. If you manage to let the children survive, you win."
    };


    private void Start()
    {
        _currentInfo = _txtInfo_1;
        ShowTutorialPanel(_currentInfo[0]);
    }
    private void Update()
    {
        if (_active && Input.anyKeyDown) NextInfo();
    }

    void NextInfo()
    {
        _index++;
        if (_currentInfo.Length == _index) ExitTutorialPanel();
        else SetTutorialInfo(_currentInfo[_index]);
    }
    void SetTutorialInfo(string text)
    {
        _txtTutorialInfo.text = text;

        if (text == _txtInfo_1[1])
        {
            _countdownTimer.SetActive(true);
            _btnFastForward.SetActive(true);
        }
        if (text == _txtInfo_1[2])
        {
            _btnTape.SetActive(true);
            _btnMove.SetActive(true);   
        }
    }

    public void ShowTutorialPanel(string text)
    {
        Time.timeScale = 0;
        _inGameUi.enabled = false;

        _txtTutorialInfo.text = text;
        //_audio.Play();
        _tutorialInfo.SetActive(true);

        canvas.SetWidgetsActive(false);

        _active = true;
    }


    void ExitTutorialPanel()
    {
        Time.timeScale = 1;
        _inGameUi.enabled = true;

        //_audio.Play();
        _tutorialInfo.SetActive(false);
        _inGameMenuObject.SetActive(true);

        canvas.SetWidgetsActive(true);

        _index = 0;
        _active = false;
    }

    public void NextPhase()
    {
        _currentInfo = _txtInfo_2;
        ShowTutorialPanel(_currentInfo[0]);
    }

}
