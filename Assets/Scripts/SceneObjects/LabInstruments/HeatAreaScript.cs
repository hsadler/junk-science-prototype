using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatAreaScript : MonoBehaviour
{


    public float heatApplied = 0f;

    public bool useHeatFieldSounds;
    public AudioSource heatFieldEnterAS;
    public AudioSource heatFieldExitAS;


    // UNITY HOOKS

    void Start() { }

    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        // handle science element
        var seColliderScript = other.gameObject.GetComponent<ScienceElementColliderScript>();
        if (seColliderScript != null)
        {
            seColliderScript.seScript.isReceivingHeat = true;
            seColliderScript.seScript.receivingHeatAmount = this.heatApplied;
        }
        // handle beaker
        bool doBeakerEnterSound = (
            this.useHeatFieldSounds &&
            other.gameObject.CompareTag("beaker") &&
            other.gameObject.GetComponent<Rigidbody>().isKinematic
        );
        if (doBeakerEnterSound)
        {
            this.heatFieldEnterAS.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // handle science element
        var seColliderScript = other.gameObject.GetComponent<ScienceElementColliderScript>();
        if (seColliderScript != null)
        {
            seColliderScript.seScript.isReceivingHeat = false;
            seColliderScript.seScript.receivingHeatAmount = 0f;
        }
        // handle beaker
        bool doBeakerExitSound = (
            this.useHeatFieldSounds &&
            other.gameObject.CompareTag("beaker") &&
            other.gameObject.GetComponent<Rigidbody>().isKinematic
        );
        if (doBeakerExitSound)
        {
            this.heatFieldExitAS.Play();
        }
    }

    // IMPLEMENTATION METHODS

}
