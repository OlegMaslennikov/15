using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] musicSet = new AudioClip[2];
    public int currentTrack = 0;
    private bool _musicIsPaused = true;

    public void Start()
    {
        GetComponent<AudioSource>().clip = musicSet[currentTrack];
        GetComponent<AudioSource>().Play();
    }

    public void Update()
    {
        if (GetComponent<AudioSource>().time == musicSet[currentTrack].length) 
        {
            ChangeTrack();
        }
    }

    public void ChangeTrack()
    {
        currentTrack += 1;
        if (currentTrack == musicSet.Length)
        {
            currentTrack = 0;
        }
        GetComponent<AudioSource>().clip = musicSet[currentTrack];
        GetComponent<AudioSource>().Play();
    }

    public void pauseMusic()
    {
        _musicIsPaused = !_musicIsPaused;
        Debug.Log(_musicIsPaused);

        if (_musicIsPaused)
            GetComponent<AudioSource>().UnPause();
        else GetComponent<AudioSource>().Pause();
    }

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
