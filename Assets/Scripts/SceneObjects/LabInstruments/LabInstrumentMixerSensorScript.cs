using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabInstrumentMixerSensorScript : MonoBehaviour
{


    public LabInstrumentMixerScript labInstrumentMixerScript;


    // UNITY HOOKS

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("LabInstrumentMixerSensorScript OnCollisionEnter()");
        bool mixable = (collision.gameObject.GetComponent<MixableScript>() != null);
        if (mixable && this.labInstrumentMixerScript.mixGO == null)
        {
            Debug.Log("Setting GO to be mixed..");
            this.labInstrumentMixerScript.mixGO = collision.gameObject;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Unsetting GO to be mixed..");
        this.labInstrumentMixerScript.mixGO = null;
    }


}
