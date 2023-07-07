using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSoundLogic : MonoBehaviour
{
    [SerializeField] AudioSource MenuClicks;

    public void clicked(){
        MenuClicks.Play();
    }


   
}
