using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial_Handler : MonoBehaviour
{
    [SerializeField] Canvas_Script canvas;
    [SerializeField] InGameUI _inGameUi;
    [SerializeField] Raycast raycast;
    [SerializeField] GameObject _gameOverPanel;
    public GameObject _tutorialInfo;
    public GameObject _inGameMenuObject;
    public GameObject _countdownTimer, _btnFastForward, _btnTape, _btnMove, _btnConsumable;
    public AudioSource _audio;
    [SerializeField] TMP_Text _txtTutorialInfo;

    int _index;
    bool _active;
    bool _childDead, _firstInteraction, _won;
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
    string[] _txtInfo_3 =
    {
        "Well, seems like the child died.",
        "Try to avoid the child from playing with dangerous objects by clicking on them when the child wants to interact."
    };
    string[] _txtInfo_4 =
    {
        "Alright, the child is interessted in this object...",
        "You will now see a timer-widget, going down slowly",
        "If you think, the object might be dangerous to the child, click and hold on the object to rewind the timer until it is filled up again!"
    };
    string[] _txtInfo_5 =
    {
        "Hey, good job so far! These objects were indeed dangerous.",
        "Heres a joker for you. If it your job get's really messy, use the cookies to distract a child quickly.",
        "Usually I'd say: Use them wisely. But due to the fact that this is a tutorial, you're free to try this cookie out by now.",
        "A good timing would be when the child starts a new interaction.",
        "Alright, now try to finish this level"
    };
    string[] _txtInfo_6 =
    {
        "You made it! Good job!",
        "Now you're ready. Have fun playing the game!"
    };


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
        else if (text == _txtInfo_1[2])
        {
            _btnTape.SetActive(true);
            _btnMove.SetActive(true);   
        }
        else if (text == _txtInfo_5[1])
            _btnConsumable.SetActive(true);

    }

    public void ShowTutorialPanel(string text)
    {
        Time.timeScale = 0;
        raycast.enabled = false;
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
        raycast.enabled = true;
        _inGameUi.enabled = true;

        //_audio.Play();
        _tutorialInfo.SetActive(false);
        _inGameMenuObject.SetActive(true);

        canvas.SetWidgetsActive(true);

        _index = 0;
        _active = false;

        if(_childDead)
        {
            _gameOverPanel.SetActive(true);
            _childDead = false;
        }

        else if(_won)
        {
            SceneManager.LoadScene(1);
        }
    }


    public void PrepPhase()
    {
        _firstInteraction = true;

        _countdownTimer.SetActive(false);
        _btnFastForward.SetActive(false);
        _btnTape.SetActive(false);
        _btnMove.SetActive(false);
        _btnConsumable.SetActive(false);

        _currentInfo = _txtInfo_1;
        ShowTutorialPanel(_currentInfo[0]);
    }

    public void NextPhase()
    {
        _currentInfo = _txtInfo_2;
        ShowTutorialPanel(_currentInfo[0]);
    }

    public void ChildDied() 
    {
        _childDead = true;
        _currentInfo = _txtInfo_3;
        ShowTutorialPanel(_currentInfo[0]);
    }

    public void InteractionStarted()
    {
        if (_firstInteraction)
        {
            _firstInteraction = false;
            _currentInfo = _txtInfo_4;
            ShowTutorialPanel(_currentInfo[0]);
        }
    }

    public void TimerRewinded()
    {
        _currentInfo = _txtInfo_5;
        ShowTutorialPanel(_currentInfo[0]);
    }

    public void Won()
    {
        _won = true;
        _currentInfo = _txtInfo_6;
        ShowTutorialPanel(_currentInfo[0]);
    }
}
