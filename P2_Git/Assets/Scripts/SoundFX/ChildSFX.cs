using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSFX : MonoBehaviour
{
    [SerializeField] List<AudioClip> leftSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> rightSteps = new List<AudioClip>();
    [SerializeField]AudioSource m_AudioSource;


    public void leftStep(){
        int lIndex = 0;
        foreach(AudioClip sound in leftSteps){
            lIndex += 1;
        }
        m_AudioSource.clip = leftSteps[Random.Range(0, lIndex)];
        m_AudioSource.Play();
    }
    public void rightStep(){
        int rIndex = 0;
        foreach(AudioClip sound in rightSteps){
            rIndex += 1;
        }
        m_AudioSource.clip = rightSteps[Random.Range(0, rIndex)];
        m_AudioSource.Play();
    }
}
