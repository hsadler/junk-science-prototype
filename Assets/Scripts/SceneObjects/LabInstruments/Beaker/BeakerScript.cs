using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakerScript : MonoBehaviour
{


    public AudioSource hardImpactSoundAS;
    public AudioSource lightImpactSoundAS;


    // UNITY HOOKS

    void Start() { }

    void Update() { }

    void OnCollisionEnter(Collision collision)
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
