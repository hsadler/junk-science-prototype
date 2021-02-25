using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceElementScript : MonoBehaviour {


    public bool activated = false;


    void Start() {}

    void Update() {}

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collided with gameObject.name: " + collision.gameObject.name);
        //if(!this.activated && collision.gameObject.name)
    }


}
