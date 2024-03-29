using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{


    public GameSound[] gameSounds;
    private IDictionary<string, GameSound> nameToGameSound = new Dictionary<string, GameSound>();

    public float musicVolume;
    public bool musicMuted = false;


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
        GameSound music = this.GetSoundByName("music");
        music.audioSource.volume = this.musicVolume;
        music.audioSource.loop = true;
        music.audioSource.Play();
    }

    void Update() { }

    // INTERFACE METHODS

    public GameSound GetSoundByName(string name)
    {
        if (nameToGameSound.ContainsKey(name))
        {
            return nameToGameSound[name];
        }
        else
        {
            return null;
        }
    }

    public void MuteMusic()
    {
        GameSound music = this.GetSoundByName("music");
        music.audioSource.volume = 0f;
        this.musicMuted = true;
    }

    public void UnmuteMusic()
    {
        GameSound music = this.GetSoundByName("music");
        music.audioSource.volume = this.musicVolume;
        this.musicMuted = false;
    }

    public void ToggleMuteMusic()
    {
        if (this.musicMuted)
        {
            this.UnmuteMusic();
        }
        else
        {
            this.MuteMusic();
        }
    }


}
