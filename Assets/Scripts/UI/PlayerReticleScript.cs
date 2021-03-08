using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReticleScript : MonoBehaviour
{


    public GameObject PlayerReticleDot;


    // UNITY HOOKS

    void Start()
    {
        LabSceneManager.instance.playerSetActive.AddListener(this.ActivateDot);
        LabSceneManager.instance.playerSetInactive.AddListener(this.DeactivateDot);
    }

    void Update()
    {
        
    }

    // IMPLEMENTATION METHODS

    private void ActivateDot()
    {
        PlayerReticleDot.SetActive(true);
    }

    private void DeactivateDot()
    {
        PlayerReticleDot.SetActive(false);
    }


}
