using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSFX : MonoBehaviour
{
    [SerializeField] List<AudioClip> m_AudioClips;
    AudioSource m_AudioSource;


    public void InteractionIdle(){
        m_AudioSource.clip = m_AudioClips[0];
        m_AudioSource.Play();
    }
    public void InteractionDeath(){
        m_AudioSource.clip = m_AudioClips[1];
        m_AudioSource.Play();
    }
}
