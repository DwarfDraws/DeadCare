using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSFX : MonoBehaviour
{
    [SerializeField] List<AudioClip> m_Idles;
    [SerializeField] List<AudioClip> m_Deaths;
    [SerializeField] List<AudioClip> m_Tapes;
    [SerializeField]AudioSource m_AudioSource;



    public void InteractionIdle(){
        int idleIndex = 0;
        foreach(AudioClip sound in m_Idles){
            idleIndex += 1;
        }
        Debug.Log(idleIndex);
        m_AudioSource.clip = m_Idles[Random.Range(0, idleIndex)];
        m_AudioSource.Play();
    }
    public void InteractionDeath(){
        int deathIndex = 0;
        foreach(AudioClip sound in m_Deaths){
            deathIndex += 1;
        }
        m_AudioSource.clip = m_Deaths[Random.Range(0, deathIndex)];
        m_AudioSource.Play();
    }
    public void InteractionTape(){
        int tapeIndex = 0;
        foreach(AudioClip sound in m_Tapes){
            tapeIndex += 1;
        }
        m_AudioSource.clip = m_Deaths[Random.Range(0, tapeIndex)];
        m_AudioSource.Play();
    }
}
