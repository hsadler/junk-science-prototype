using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserButtonManagerScript : MonoBehaviour
{


    public List<DispenserButtonScript> dispenserButtons;
    public SEInletScript seInletScript;

    public AudioSource buttonPressAS;
    public AudioSource dispenserRunningAS;


    // UNITY HOOKS

    void Start() { }

    void Update() { }

    // INTERFACE METHODS

    public void TurnOnButton(DispenserButtonScript button)
    {
        foreach (var dbScript in this.dispenserButtons)
        {
            dbScript.TurnButtonOff();
        }
        button.TurnButtonOn();
        this.buttonPressAS.Play();
        this.seInletScript.TurnOn(
            scienceElementTag: button.scienceElementTag,
            spawnRowLength: 2,
            spawnColumnLength: 2,
            itemSpread: 1f,
            propulsionForce: 20f,
            secondsPerSpawn: 0.2f
        );
        this.dispenserRunningAS.Play();
    }

    public void TurnOffButton(DispenserButtonScript button)
    {
        button.TurnButtonOff();
        this.buttonPressAS.Play();
        this.seInletScript.TurnOff();
        this.dispenserRunningAS.Stop();
    }


}
