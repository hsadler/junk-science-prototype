using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceElementSoundsScript : MonoBehaviour
{


    public AudioSource audioSource;

    // pop sounds for SE creation
    public AudioClip[] popAudioClips;
    public float popVolume;
    public float popPitch;
    public float popPitchVariation;

    // collision sounds for SE+SE collisions
    public AudioClip[] collisionAudioClips;
    public float collisionVolume;
    public float collisionPitch;
    public float collisionPitchVariation;

    // sizzle sounds for SE heating reactions
    public AudioClip[] sizzleAudioClips;
    public float sizzleVolume;
    public float sizzlePitch;
    public float sizzlePitchVariation;

    // ice crack sounds for SE cooling reactions
    public AudioClip[] iceCrackAudioClips;
    public float iceCrackVolume;
    public float iceCrackPitch;
    public float iceCrackPitchVariation;

    // impact sounds for SE table, floor, etc hits
    public AudioClip[] impactAudioClips;
    public float impactVolume;
    public float impactPitch;
    public float impactPitchVariation;


    // UNITY HOOKS

    void Start() { }

    void Update() { }

    // INTERFACE METHODS

    public void PlayCreateSound()
    {
        if (this.popAudioClips.Length > 0)
        {
            // select a random pop clip, randomly adjust pitch within a variation window 
            AudioClip clip = this.GetRandomClipFromCollection(this.popAudioClips);
            this.audioSource.pitch = this.GetRandomAdjustedPitch(this.popPitch, this.popPitchVariation);
            this.audioSource.volume = this.popVolume;
            this.audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("There are no science-element pop audio clips to choose from");
        }
    }

    public void PlayDestroySound()
    {
        // MAY NOT USE THIS
        // Debug.Log("PlayDestroySound...");
    }

    public void PlayCollisionReactionSound()
    {
        // TODO: implement stub
        Debug.Log("PlayCollisionReactionSound...");
    }

    public void PlayHeatingReactionSound()
    {
        if (this.sizzleAudioClips.Length > 0)
        {
            // select a random sizzle clip, randomly adjust pitch within a variation window 
            AudioClip clip = this.GetRandomClipFromCollection(this.sizzleAudioClips);
            this.audioSource.pitch = this.GetRandomAdjustedPitch(this.sizzlePitch, this.sizzlePitchVariation);
            this.audioSource.volume = this.sizzleVolume;
            this.audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("There are no science-element sizzle audio clips to choose from");
        }
    }

    public void PlayCoolingReactionSound()
    {
        if (this.iceCrackAudioClips.Length > 0)
        {
            // select a random ice-crack clip, randomly adjust pitch within a variation window 
            AudioClip clip = this.GetRandomClipFromCollection(this.iceCrackAudioClips);
            this.audioSource.pitch = this.GetRandomAdjustedPitch(this.iceCrackPitch, this.iceCrackPitchVariation);
            this.audioSource.volume = this.iceCrackVolume;
            this.audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("There are no science-element ice-crack audio clips to choose from");
        }
    }

    public void PlayImpactSound()
    {
        if (this.impactAudioClips.Length > 0)
        {
            // select a random impact clip, randomly adjust pitch within a variation window 
            AudioClip clip = this.GetRandomClipFromCollection(this.impactAudioClips);
            this.audioSource.pitch = this.GetRandomAdjustedPitch(this.impactPitch, this.impactPitchVariation);
            this.audioSource.volume = this.impactVolume;
            this.audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("There are no science-element impact audio clips to choose from");
        }
    }

    // IMPLEMENTATION METHODS

    private AudioClip GetRandomClipFromCollection(AudioClip[] audioClips)
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }

    private float GetRandomAdjustedPitch(float pitch, float variation)
    {
        float adjustAmount = Random.Range(-variation / 2, variation / 2);
        return pitch + adjustAmount;
    }


}
