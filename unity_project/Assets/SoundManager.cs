using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour, Timer
{

    public List<AudioClip> loopingMusic;


    public AudioSource sourceOne;
    public AudioSource sourceTwo;

    public AudioMixerSnapshot snapOne;
    public AudioMixerSnapshot snapTwo;

    public static SoundManager thisM;

    bool playingOne = true;

    AudioSource current;
    AudioMixerSnapshot currentShot;

    public float m_Transition;

    void Awake()
    {
        thisM = this;
    }


    // Use this for initialization
    void Start()
    {
        Play();
    }

    public void CancelTimer()
    {

    }

    public void Play()
    {
        int randClip = UnityEngine.Random.Range(0, loopingMusic.Count);

        if (playingOne)
        {
            current = sourceOne;
            currentShot = snapOne;
        }
        else
        {
            current = sourceTwo;
            currentShot = snapTwo;
        }

        currentShot.TransitionTo(m_Transition / 2);

        current.clip = loopingMusic[randClip];
        if (current.clip == null)
        {
            Invoke("Play", 60f);
            return;
        }
        current.Play();
        StartTimer(current.clip.length - m_Transition);
    }

    public void StartTimer(float time)
    {
        Invoke("TimerComplete", time);
    }

    public void TimerComplete()
    {
        playingOne = !playingOne;
        currentShot.TransitionTo(m_Transition);
        Invoke("Play", m_Transition / 2);
    }
}
