using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceElementSoundsScript : MonoBehaviour
{


    public AudioSource audioSource;
    public AudioClip[] popAudioClips;

    public float popPitch;
    public float popPitchVariation;


    // UNITY HOOKS

    void Start() { }

    void Update() { }

    // INTERFACE METHODS

    public void PlayCreateSound()
    {
        if (this.popAudioClips.Length > 0)
        {
            // select a random pop clip, randomly adjust pitch within a variation window 
            AudioClip randPopClip = popAudioClips[Random.Range(0, this.popAudioClips.Length)];
            float adjustValue = Random.Range(-this.popPitchVariation / 2, this.popPitchVariation / 2);
            float adjustedPitch = this.popPitch + adjustValue;
            this.audioSource.pitch = adjustedPitch;
            this.audioSource.PlayOneShot(randPopClip);
        }
        else
        {
            Debug.LogWarning("There are no science-element pop audio clips to choose from");
        }
    }

    public void PlayDestroySound()
    {
        // Debug.Log("PlayDestroySound...");
    }

    public void PlayCollisionReactionSound()
    {
        // TODO: implement stub
        Debug.Log("PlayCollisionReactionSound...");
    }

    public void PlayHeatingReactionSound()
    {
        // TODO: implement stub
        Debug.Log("PlayHeatingReactionSound...");
    }

    public void PlayCoolingReactionSound()
    {
        // TODO: implement stub
        Debug.Log("PlayCoolingReactionSound...");
    }


}
