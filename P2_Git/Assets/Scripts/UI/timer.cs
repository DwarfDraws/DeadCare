using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public Image timerbar;
    public float speed;
    public float speed2;
    public bool runtimer = true;

    void Start()
    {
        
    }

    void Update()
    {



        if (Input.GetMouseButton(0))
        {
            runtimer = false;

            timerbar.fillAmount += speed2;
        }
        else
        {
            if (timerbar.fillAmount > 0)
            {
                timerbar.fillAmount -= speed;
            }
        }
    }

}
