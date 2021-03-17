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
        bool isSE = (other.gameObject.GetComponent<ScienceElementScript>() != null);
        if (isSE)
        {
            other.gameObject.SetActive(false);
            LabSceneManager.instance.GiveScienceElementBackToPool(other.gameObject);
        }
    }


}
