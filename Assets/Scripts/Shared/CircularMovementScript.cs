using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovementScript : MonoBehaviour
{

    // Applies circular movement to a game object


    public bool isActive = false;
    public Transform rotationCenter;
    public float rotationRadius = 2f;
    public float angularSpeed = 2f;

    private float posX, posZ, angle = 0f;


    public void Update() {
        if(isActive) {
            CalculatePosition();
        }
    }

    private void CalculatePosition() {
        posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
        posZ = rotationCenter.position.z + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector3(posX, transform.position.y, posZ);
        angle = angle + Time.deltaTime * angularSpeed;
        if(angle >= 360f) {
            angle = 0f;
        }
    }


}
