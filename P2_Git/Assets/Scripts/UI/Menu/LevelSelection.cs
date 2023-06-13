using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    // Start is called before the first frame update
   public void SelectLevel01()
    {
        SceneManager.LoadScene("Scene_Easy");
    }
    public void SelectLevel02()
    {
        SceneManager.LoadScene("Scene_Middle 1");
    }
    public void SelectLevel03()
    {
        SceneManager.LoadScene("Scene_Middle 2");
    }
    public void SelectLevel04()
    {
        SceneManager.LoadScene("Scene_Hard 1");
    }
    public void SelectLevel05()
    {
        SceneManager.LoadScene("Scene_Hard 2");
    }
}
