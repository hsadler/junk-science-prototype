using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabInstrumentHeaterButtonScript : MonoBehaviour
{


    public LabInstrumentHeaterScript labInstrumentHeaterScript;
    public MeshRenderer meshRenderer;


    // UNITY HOOKS

    void Start()
    {
        this.meshRenderer = GetComponent<MeshRenderer>();
        this.meshRenderer.material = labInstrumentHeaterScript.buttonOffMaterial;
    }

    void Update()
    {
        
    }

    // INTERFACE METHODS

    public void PlayerLeftClickInteraction(PlayerInteractionMessage message)
    {
        if (gameObject == message.hit.collider.gameObject)
        {
            this.labInstrumentHeaterScript.ToggleOnOff();
        }
    }


}
