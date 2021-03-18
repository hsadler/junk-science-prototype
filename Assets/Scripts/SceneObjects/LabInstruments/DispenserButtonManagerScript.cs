using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserButtonManagerScript : MonoBehaviour
{


    public List<DispenserButtonScript> dispenserButtons;
    public SEInletScript seInletScript;


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
        foreach(var dbScript in this.dispenserButtons)
        {
            dbScript.TurnButtonOff();
        }
        button.TurnButtonOn();
        seInletScript.TurnOn(
            scienceElementTag: button.scienceElementTag,
            spawnObjectScale: 1f,
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
        seInletScript.TurnOff();
    }


}
