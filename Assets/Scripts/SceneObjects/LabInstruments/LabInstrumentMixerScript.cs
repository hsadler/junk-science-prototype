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

    // rotation
    private float rotateSpeed = 60f;
    private float rotateSmooth = 3f;
    private Vector3 currentRotationVector = Vector3.zero;


    // UNITY HOOKS

    void Start()
    {
        InvokeRepeating("ChangeRotationVector", 0f, 0.2f);
    }

    void Update()
    {
        if(this.isOn)
        {
            this.Mix();
        }
    }

    // INTERFACE METHODS

    public void TurnOn()
    {
        this.isOn = true;
        this.labInstrumentMixerButtonScript.meshRenderer.material = this.buttonOnMaterial;
    }

    public void TurnOff()
    {
        this.isOn = false;
        this.labInstrumentMixerButtonScript.meshRenderer.material = this.buttonOffMaterial;
    }

    public void ToggleOnOff()
    {
        if (!this.isOn && this.mixGO != null)
        {
            this.TurnOn();
        }
        else if(this.isOn)
        {
            this.TurnOff();
        }

    }

    // IMPLEMENTATION METHODS

    private void Mix()
    {
        Quaternion toRotation = this.mixGO.transform.localRotation *
            Quaternion.AngleAxis(this.rotateSpeed, this.currentRotationVector);
        this.mixGO.transform.rotation = Quaternion.Lerp(
            this.mixGO.transform.rotation,
            toRotation,
            Time.deltaTime * this.rotateSmooth
        );
    }

    private void ChangeRotationVector()
    {
        if (this.currentRotationVector == Vector3.forward)
        {
            this.currentRotationVector = Vector3.back;
        } else
        {
            this.currentRotationVector = Vector3.forward;
        }
    }


}
