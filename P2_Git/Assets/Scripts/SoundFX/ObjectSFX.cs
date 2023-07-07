using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSFX : MonoBehaviour
{
    [SerializeField] List<AudioClip> m_Idles = new List<AudioClip>();
    [SerializeField] List<AudioClip> m_Deaths = new List<AudioClip>();
    [SerializeField] List<AudioClip> m_AddTapes = new List<AudioClip>();
    [SerializeField] List<AudioClip> m_RemoveTapes = new List<AudioClip>();
    [SerializeField] AudioClip doorOpen, doorClose;
    [SerializeField]AudioSource m_AudioSource;

    int idlesCount, deathsCount, addTapesCount, removeTapesCount;

    private void Start() {
        idlesCount = m_Idles.Count;
        deathsCount = m_Deaths.Count;
        addTapesCount = m_AddTapes.Count;
        removeTapesCount = m_RemoveTapes.Count;
    }

    public void DoorOpen(){
        m_AudioSource.clip = doorOpen;
        m_AudioSource.Play();
    }

    public void DoorClose(){
        m_AudioSource.clip = doorClose;
        m_AudioSource.Play();        
    }

    public void InteractionIdle(){
        m_AudioSource.clip = m_Idles[Random.Range(0, idlesCount)];
        m_AudioSource.Play();
    }
    public void InteractionDeath(){
        m_AudioSource.clip = m_Deaths[Random.Range(0, deathsCount)];
        m_AudioSource.Play();
    }
    public void InteractionAddTape(){
        m_AudioSource.clip = m_AddTapes[Random.Range(0, addTapesCount)];
        m_AudioSource.Play();
    }
    public void InteractionRemoveTape(){
        m_AudioSource.clip = m_RemoveTapes[Random.Range(0, removeTapesCount)];
        m_AudioSource.Play();
        }
        
}
