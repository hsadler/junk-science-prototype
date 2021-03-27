using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserButtonScript : MonoBehaviour
{


    public string scienceElementTag;
    public DispenserButtonManagerScript dispenserButtonManagerScript;
    public Material materialOn;
    public Material materialOff;

    private bool _isOn;
    private MeshRenderer _mr;


    // UNITY HOOKS

    void Start()
    {
        _isOn = false;
        _mr = GetComponent<MeshRenderer>();
        _mr.material = this.materialOff;
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public void PlayerLeftClickInteraction(PlayerInteractionMessage message)
    {
        if (gameObject == message.hit.collider.gameObject)
        {
            if (_isOn)
            {
                dispenserButtonManagerScript.TurnOffButton(this);

            }
            else
            {
                dispenserButtonManagerScript.TurnOnButton(this);
            }
        }
    }

    public void TurnButtonOn()
    {
        _isOn = true;
        _mr.material = this.materialOn;
    }

    public void TurnButtonOff()
    {
        _isOn = false;
        _mr.material = this.materialOff;
    }

}
