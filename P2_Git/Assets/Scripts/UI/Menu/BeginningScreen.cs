using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginningScreen : MonoBehaviour
{

    [SerializeField] PersistentSoundLogic pSL;
    private void Update(){
        
        if(Input.anyKey){
            pSL.clicked();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
}
