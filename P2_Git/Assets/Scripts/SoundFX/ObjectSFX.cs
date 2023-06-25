using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSFX : MonoBehaviour
{
    [SerializeField] List<AudioClip> m_Idles = new List<AudioClip>();
    [SerializeField] List<AudioClip> m_Deaths = new List<AudioClip>();
    [SerializeField] List<AudioClip> m_AddTapes = new List<AudioClip>();
    [SerializeField] List<AudioClip> m_RemoveTapes = new List<AudioClip>();
    [SerializeField]AudioSource m_AudioSource;
    private bool removedTape = false;



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
    public void InteractionAddTape(){
        int tapeIndex = 0;
        foreach(AudioClip sound in m_AddTapes){
            tapeIndex += 1;
        }
        m_AudioSource.clip = m_AddTapes[Random.Range(0, tapeIndex)];
        m_AudioSource.Play();
    }
    public void InteractionRemoveTape(){
        Debug.Log("repeat");
        if(!removedTape){
            int tapeIndex = 0;
            foreach(AudioClip sound in m_RemoveTapes){
                tapeIndex += 1;
        }
        m_AudioSource.clip = m_RemoveTapes[Random.Range(0, tapeIndex)];
        m_AudioSource.Play();
        removedTape = true;
        }
        
    }
}
