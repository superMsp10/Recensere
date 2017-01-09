using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Audio;
using System.Collections.Generic;

public class Level1Music : MonoBehaviour, Timer
{
    public List<AudioClip> loopingMusic;

    public AudioSource source;

    public AudioMixerSnapshot MusicOnly;
    public AudioMixerSnapshot NoiseOnly;
    public AudioMixerSnapshot noSounds;


    public void CancelTimer()
    {
        throw new NotImplementedException();
    }

    public void StartTimer(float time)
    {
        Invoke("TimerComplete", time);
    }

    public void TimerComplete()
    {
        Invoke("PlayRandom", 30f);
        NoiseOnly.TransitionTo(1f);

    }

    // Use this for initialization
    void Start()
    {
        PlayRandom();
    }

    void PlayRandom()
    {
        int r = UnityEngine.Random.Range(0, 5);
        if (!(r == 1))
        {
            Invoke("PlayRandom", 60 * 5f);
            NoiseOnly.TransitionTo(2f);
        }
        else
        {
            int randClip = UnityEngine.Random.Range(0, loopingMusic.Count);
            source.clip = loopingMusic[randClip];
            source.Play();
            StartTimer(source.clip.length);
            MusicOnly.TransitionTo(2f);
            noSounds.TransitionTo(1f);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
