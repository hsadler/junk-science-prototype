using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabInstrumentMixerFieldScript : MonoBehaviour
{


    public LabInstrumentMixerScript labInstrumentMixerScript;


    // UNITY HOOKS

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {

    }

    void OnTriggerExit(Collider other)
    {
        // TODO: FIGURE OUT HOW TO GET THIS TO WORK
        // object exiting collision is same object currently being mixed
        //if (other.gameObject == this.labInstrumentMixerScript.mixGO)
        //{
        //    this.labInstrumentMixerScript.mixGO = null;
        //    this.labInstrumentMixerScript.TurnOff();
        //}
    }


}
