using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumables : MonoBehaviour
{
    [SerializeField] Settings_script settings;
    public float existenceTimeSeconds;
    float timer;
    Canvas_Script canvas;
    Widget widget;

    bool timeStart;


    private void Start() 
    {
        timer = existenceTimeSeconds;
        settings = GameObject.Find("Settings").GetComponent<Settings_script>();

        if(settings.consumablesHaveExistenceTimer)
        {
            Vector3 widget_pos = this.transform.GetChild(0).gameObject.transform.position;

            canvas = GameObject.Find("Canvas").GetComponent<Canvas_Script>();
            widget = canvas.InstantiateWidget(widget_pos, existenceTimeSeconds, Color.blue);
            StartExistenceTimer(true);
        }
    }

    private void Update() 
    {
        if(timeStart) Timer();
    }


    public void Timer()
    {
        timer -= Time.deltaTime;
        widget.UpdateWidget(timer);

        if(timer <= 0)
        {       
            Destroy(widget.gameObject);
            Destroy(gameObject);
        }
    }


    public void StartExistenceTimer(bool isActivated)
    {
        if(isActivated) timeStart = true;
        else
        {
            timeStart = false;
            Destroy(widget.gameObject);
        } 
    }
}
