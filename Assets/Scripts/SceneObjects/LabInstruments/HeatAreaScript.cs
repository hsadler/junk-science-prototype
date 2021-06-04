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
        var seColliderScript = other.gameObject.GetComponent<ScienceElementColliderScript>();
        if (seColliderScript == null) {
            return;
        }
        seColliderScript.seScript.receivingHeat = true;
        seColliderScript.seScript.receivingHeatAmount = this.heatApplied;
    }

    void OnTriggerExit(Collider other)
    {
        var seColliderScript = other.gameObject.GetComponent<ScienceElementColliderScript>();
        if (seColliderScript == null) {
            return;
        }
        seColliderScript.seScript.receivingHeat = false;
        seColliderScript.seScript.receivingHeatAmount = 0f;
    }

    // IMPLEMENTATION METHODS

}
