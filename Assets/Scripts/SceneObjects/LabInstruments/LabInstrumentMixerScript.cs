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
        }
        else
        {
            this.isOn = true;
            this.labInstrumentMixerButtonScript.meshRenderer.material = this.buttonOnMaterial;
        }

    }


}
