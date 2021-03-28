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
    public Material heatSurfaceOnMaterial;
    public Material heatSurfaceOffMaterial;

    // heat surface
    public GameObject heatSurfaceGO;
    private MeshRenderer heatSurfaceMeshRenderer;

    // heat area
    public GameObject heatAreaGO;
    private HeatAreaScript heatAreaScript;


    // UNITY HOOKS

    void Start()
    {
        this.heatSurfaceMeshRenderer = heatSurfaceGO.GetComponent<MeshRenderer>();
        this.heatAreaScript = heatAreaGO.GetComponent<HeatAreaScript>();
        this.heatAreaGO.SetActive(false);
    }

    void Update()
    {
        
    }

    // INTERFACE METHODS

    public void TurnOn()
    {
        this.isOn = true;
        this.labInstrumentHeaterButtonScript.meshRenderer.material = this.buttonOnMaterial;
        this.heatSurfaceMeshRenderer.material = this.heatSurfaceOnMaterial;
        this.heatAreaGO.SetActive(true);
    }

    public void TurnOff()
    {
        this.isOn = false;
        this.labInstrumentHeaterButtonScript.meshRenderer.material = this.buttonOffMaterial;
        this.heatSurfaceMeshRenderer.material = this.heatSurfaceOffMaterial;
        this.heatAreaGO.SetActive(false);
    }

    public void ToggleOnOff()
    {
        if (!this.isOn)
        {
            this.TurnOn();
        }
        else if(this.isOn)
        {
            this.TurnOff();
        }

    }

    // IMPLEMENTATION METHODS
    

}
