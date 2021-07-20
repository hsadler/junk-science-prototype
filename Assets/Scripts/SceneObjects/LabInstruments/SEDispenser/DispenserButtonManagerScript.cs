using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserButtonManagerScript : MonoBehaviour
{


    public List<DispenserButtonScript> dispenserButtons;
    public SEInletScript seInletScript;

    public AudioSource buttonPressAS;


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public void TurnOnButton(DispenserButtonScript button)
    {
        foreach (var dbScript in this.dispenserButtons)
        {
            dbScript.TurnButtonOff();
        }
        button.TurnButtonOn();
        this.buttonPressAS.Play();
        seInletScript.TurnOn(
            scienceElementTag: button.scienceElementTag,
            spawnRowLength: 2,
            spawnColumnLength: 2,
            itemSpread: 1f,
            propulsionForce: 20f,
            secondsPerSpawn: 0.2f
        );
    }

    public void TurnOffButton(DispenserButtonScript button)
    {
        button.TurnButtonOff();
        this.buttonPressAS.Play();
        seInletScript.TurnOff();
    }


}
