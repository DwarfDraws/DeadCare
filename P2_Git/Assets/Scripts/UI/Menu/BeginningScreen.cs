using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginningScreen : MonoBehaviour
{
    private void Update(){
        if(Input.anyKey)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
