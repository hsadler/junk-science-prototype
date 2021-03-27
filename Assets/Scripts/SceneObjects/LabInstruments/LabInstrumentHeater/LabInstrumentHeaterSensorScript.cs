using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabInstrumentHeaterSensorScript : MonoBehaviour
{


    public LabInstrumentHeaterScript labInstrumentHeaterScript;

    private float distanceThreshold = 15f;


    // UNITY HOOKS

    void Start()
    {
        
    }

    void Update()
    {
        if(this.labInstrumentHeaterScript.heatGO != null)
        {
            this.CheckMixableIsWithinDistanceThreshold();
        }    
    }

    void OnCollisionEnter(Collision collision)
    {
        bool mixable = (collision.gameObject.GetComponent<MixableScript>() != null);
        if (mixable)
        {
            this.labInstrumentHeaterScript.heatGO = collision.gameObject;
        }
    }

    // IMPLEMENTATION METHODS

    private void CheckMixableIsWithinDistanceThreshold()
    {
        float distance = Vector3.Distance(
            transform.position,
            this.labInstrumentHeaterScript.heatGO.transform.position
        );
        if (distance > this.distanceThreshold)
        {
            this.labInstrumentHeaterScript.heatGO = null;
            this.labInstrumentHeaterScript.TurnOff();
        }
    }


}
