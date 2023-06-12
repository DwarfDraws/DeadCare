using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumables : MonoBehaviour
{
    [SerializeField] Settings_script settings;
    Canvas_Script canvas;
    Widget widget;
    
    public float existenceTimeSeconds;
    float timer;

    bool timeStart;


    private void Start() 
    {
        timer = 1.0f;
        settings = GameObject.Find("Settings").GetComponent<Settings_script>();

        if(settings.consumablesHaveExistenceTimer)
        {
            Vector3 widget_pos = this.transform.GetChild(0).gameObject.transform.position;

            canvas = GameObject.Find("Canvas").GetComponent<Canvas_Script>();
            widget = canvas.InstantiateWidget(widget_pos, Color.blue);
            StartExistenceTimer(true);
        }
    }

    private void Update() 
    {
        if(timeStart) ExistenceTimer();
    }


    public void ExistenceTimer()
    {
        timer -= Time.deltaTime / existenceTimeSeconds;
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
