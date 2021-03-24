using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabInstrumentMixerScript : MonoBehaviour
{


    public bool isOn = false;

    // on/off button
    public LabInstrumentMixerButtonScript labInstrumentMixerButtonScript;
    public Material buttonOnMaterial;
    public Material buttonOffMaterial;

    // object to mix
    public GameObject mixGO;


    // UNITY HOOKS

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // INTERFACE METHODS

    public void ToggleOnOff()
    {
        if (this.isOn)
        {
            this.isOn = false;
            this.labInstrumentMixerButtonScript.meshRenderer.material = this.buttonOffMaterial;
            this.StopMixing();
        }
        else
        {
            this.isOn = true;
            this.labInstrumentMixerButtonScript.meshRenderer.material = this.buttonOnMaterial;
            this.StartMixing();
        }

    }

    // IMPLEMENTATION METHODS

    private void StartMixing()
    {
        // stub
    }

    private void StopMixing()
    {
        // stub
    }


}
