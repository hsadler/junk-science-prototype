using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatAreaScript : MonoBehaviour
{


    public float heatApplied = 10f;


    // UNITY HOOKS

    void Start() {}

    void Update() {}

    void OnTriggerEnter(Collider other)
    {
        var seScript = other.gameObject.GetComponent<ScienceElementScript>();
        if (seScript == null) return;
        seScript.receivingHeat = true;
        seScript.receivingHeatAmount = this.heatApplied;
    }

    void OnTriggerExit(Collider other)
    {
        var seScript = other.gameObject.GetComponent<ScienceElementScript>();
        if (seScript == null) return;
        seScript.receivingHeat = false;
        seScript.receivingHeatAmount = 0f;
    }

    // IMPLEMENTATION METHODS

}
