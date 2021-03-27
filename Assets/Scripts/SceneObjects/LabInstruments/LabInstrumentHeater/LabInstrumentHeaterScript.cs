using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabInstrumentHeaterScript : MonoBehaviour
{


    public bool isOn = false;

    // on/off button
    public LabInstrumentHeaterButtonScript labInstrumentHeaterButtonScript;
    public Material buttonOnMaterial;
    public Material buttonOffMaterial;

    // object to heat
    public GameObject heatGO;


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {
        if(this.isOn)
        {
            this.ApplyHeatToObject();
        }
    }

    // INTERFACE METHODS

    public void TurnOn()
    {
        this.isOn = true;
        this.labInstrumentHeaterButtonScript.meshRenderer.material = this.buttonOnMaterial;
    }

    public void TurnOff()
    {
        this.isOn = false;
        this.labInstrumentHeaterButtonScript.meshRenderer.material = this.buttonOffMaterial;
    }

    public void ToggleOnOff()
    {
        if (!this.isOn && this.heatGO != null)
        {
            this.TurnOn();
        }
        else if(this.isOn)
        {
            this.TurnOff();
        }

    }

    // IMPLEMENTATION METHODS

    private void ApplyHeatToObject()
    {
        // TODO: STUB
    }

}
