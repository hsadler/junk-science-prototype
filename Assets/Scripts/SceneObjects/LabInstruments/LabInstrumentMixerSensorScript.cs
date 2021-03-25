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
        bool mixable = (collision.gameObject.GetComponent<MixableScript>() != null);
        if (mixable)
        {
            this.labInstrumentMixerScript.mixGO = collision.gameObject;
        }
    }

    void OnCollisionExit(Collision collision)
    {

        // TODO: FIGURE THIS OUT SINCE WE DON'T WANT TO UNBIND AND TURN OFF WHEN
        // MIXING AND THEREFOR NO LONGER COLLIDING
        //// object exiting collision is same object currently being mixed
        //if (collision.gameObject == this.labInstrumentMixerScript.mixGO)
        //{
        //    this.labInstrumentMixerScript.mixGO = null;
        //    this.labInstrumentMixerScript.TurnOff();
        //}
    }


}
