using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabInstrumentMixerSensorScript : MonoBehaviour
{


    public LabInstrumentMixerScript labInstrumentMixerScript;

    private float distanceThreshold = 15f;


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {
        if (this.labInstrumentMixerScript.mixGO != null)
        {
            this.CheckMixableIsWithinDistanceThreshold();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        bool mixable = (collision.gameObject.GetComponent<MixableScript>() != null);
        if (mixable)
        {
            this.labInstrumentMixerScript.mixGO = collision.gameObject;
        }
    }

    // IMPLEMENTATION METHODS

    private void CheckMixableIsWithinDistanceThreshold()
    {
        float distance = Vector3.Distance(
            transform.position,
            this.labInstrumentMixerScript.mixGO.transform.position
        );
        if (distance > this.distanceThreshold)
        {
            this.labInstrumentMixerScript.mixGO = null;
            this.labInstrumentMixerScript.TurnOff();
        }
    }


}
