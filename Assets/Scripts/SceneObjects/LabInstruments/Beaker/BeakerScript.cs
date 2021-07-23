using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakerScript : MonoBehaviour
{


    public AudioSource hardImpactSoundAS;
    public AudioSource lightImpactSoundAS;

    public AudioSource clinksSoundAS;
    public AudioClip[] clinkAudioClips;
    public float clinkVolume;
    public float clinkPitch;
    public float clinkPitchVariation;


    // UNITY HOOKS

    void Start() { }

    void Update() { }

    void OnCollisionEnter(Collision collision)
    {
        // sfx for collisions with SEs
        if (collision.gameObject.GetComponent<ScienceElementScript>() != null)
        {
            if (collision.relativeVelocity.magnitude > 25)
            {
                this.PlayClinkSFX();
            }
        }
        // sfx for all other collisions
        else
        {
            // play impact sound when sufficient impact occurs
            if (collision.relativeVelocity.magnitude > 20 && collision.relativeVelocity.magnitude < 50)
            {
                this.lightImpactSoundAS.Play();
            }
            else if (collision.relativeVelocity.magnitude >= 50)
            {
                this.hardImpactSoundAS.Play();
            }
        }
    }

    // IMPLEMENTATION METHODS

    private void PlayClinkSFX()
    {
        if (this.clinkAudioClips.Length > 0)
        {
            // select a random clink clip, randomly adjust pitch within a variation window 
            AudioClip clip = this.GetRandomClipFromCollection(this.clinkAudioClips);
            this.clinksSoundAS.pitch = this.GetRandomAdjustedPitch(this.clinkPitch, this.clinkPitchVariation);
            this.clinksSoundAS.volume = this.clinkVolume;
            this.clinksSoundAS.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("There are no beaker clink audio clips to choose from");
        }
    }

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
