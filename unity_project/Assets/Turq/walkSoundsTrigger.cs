using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class walkSoundsTrigger : MonoBehaviour
{

    //SFX
    public AudioSource source;
    public List<AudioClip> walkingClips;
    public float soundMax, soundMin;
    public PhotonView photonV;

    public void randomWalkingSFX()
    {
        source.PlayOneShot(walkingClips[Random.Range(0, walkingClips.Count - 1)], Random.Range(soundMin, soundMax));

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
