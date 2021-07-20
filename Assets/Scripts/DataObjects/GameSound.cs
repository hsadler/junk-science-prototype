using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSound
{


    public string name;
    public AudioClip audioClip;
    [HideInInspector]
    public AudioSource audioSource;


    public GameSound() { }


    // INTERFACE METHODS

    public void Play()
    {
        this.audioSource.Play();
    }


}
