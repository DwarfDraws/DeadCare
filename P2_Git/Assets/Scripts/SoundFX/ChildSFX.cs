using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSFX : MonoBehaviour
{
    [SerializeField] List<AudioClip> leftSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> rightSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> bedJumps = new List<AudioClip>();
    [SerializeField] AudioClip cookieEating, electricShock;
    [SerializeField]AudioSource m_AudioSource;

    int leftStepsCount, rightStepsCount, bedJumpsCount;


    private void Start() {
        bedJumpsCount = bedJumps.Count;
        leftStepsCount = leftSteps.Count;
        rightStepsCount = rightSteps.Count;
    }

    public void leftStep(){
        m_AudioSource.clip = leftSteps[Random.Range(0, leftStepsCount)];
        m_AudioSource.Play();
    }
    public void rightStep(){
        m_AudioSource.clip = rightSteps[Random.Range(0, rightStepsCount)];
        m_AudioSource.Play();
    }

    public void CookieEating(){
        m_AudioSource.clip = cookieEating;
        m_AudioSource.Play();
    }
    public void BedJump(){
        m_AudioSource.clip = bedJumps[Random.Range(0, bedJumpsCount)];
        m_AudioSource.Play();
    }
    public void ElectricShock(){
        m_AudioSource.clip = electricShock;
        m_AudioSource.Play();
    }
}
