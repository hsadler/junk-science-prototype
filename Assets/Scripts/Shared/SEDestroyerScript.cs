using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEDestroyerScript : MonoBehaviour
{


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        var seColliderScript = other.gameObject.GetComponent<ScienceElementColliderScript>();
        if (seColliderScript != null)
        {
            var seGO = seColliderScript.seScript.gameObject;
            seGO.SetActive(false);
            LabSceneManager.instance.GiveScienceElementBackToPool(seGO);
        }
    }


}
