using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginningScreen : MonoBehaviour
{
    [SerializeField] PersistentSoundLogic pSL;


    private void Start() {
        InitVolSettings();
    }


    private void Update(){
        
        if(Input.anyKey){
            pSL.clicked();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }

    void InitVolSettings()
    {
        SaveData saveData = SaveManager.Load();
        saveData.volume = 1.0f;
        SaveManager.Save(saveData);
    }
}
