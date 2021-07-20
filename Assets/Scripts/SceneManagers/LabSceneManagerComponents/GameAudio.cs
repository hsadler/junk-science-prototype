using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{


    public GameSound[] gameSounds;
    private IDictionary<string, GameSound> nameToGameSound = new Dictionary<string, GameSound>();


    // UNITY HOOKS

    void Awake()
    {
        foreach (var s in gameSounds)
        {
            s.audioSource = this.gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            if (!nameToGameSound.ContainsKey(s.name))
            {
                this.nameToGameSound.Add(s.name, s);
            }
            else
            {
                Debug.LogError("Could not add non-unique GameSound name to GameAudio: " + s.name);
            }
        }
    }

    void Start()
    {
        this.PlaySoundByName("music");
    }

    // INTERFACE METHODS

    public void PlaySoundByName(string name)
    {
        if (nameToGameSound.ContainsKey(name))
        {
            nameToGameSound[name].Play();
        }
    }


}
