using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour
{


    public bool myBool;


    // UNITY HOOKS

    void Awake() { }

    void Start() { }

    void Update() { }

    // INTERFACE METHODS

    public void PlaySound()
    {
        Debug.Log("Playing a sound");
    }

    // IMPLEMENTATION METHODS


}
