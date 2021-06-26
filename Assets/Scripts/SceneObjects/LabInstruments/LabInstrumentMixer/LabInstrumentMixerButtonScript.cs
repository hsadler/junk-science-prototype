using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabInstrumentMixerButtonScript : MonoBehaviour
{


    public LabInstrumentMixerScript labInstrumentMixerScript;
    public MeshRenderer meshRenderer;


    // UNITY HOOKS

    void Start()
    {
        this.meshRenderer = GetComponent<MeshRenderer>();
        this.meshRenderer.material = labInstrumentMixerScript.buttonOffMaterial;
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public void PlayerLeftClickInteraction(PlayerInteractionMessage message)
    {
        if (gameObject == message.hit.collider.gameObject)
        {
            this.labInstrumentMixerScript.ToggleOnOff();
        }
    }


}
