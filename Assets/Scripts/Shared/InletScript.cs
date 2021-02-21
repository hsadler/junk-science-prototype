using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InletScript : MonoBehaviour
{

    // Spawner for continuous-flow object creation


    public GameObject spawnObject;
    public Transform spawnPointTransform;
    public int spawnRowLength = 10;
    public int spawnColumnLength = 10;
    public float propulsionForce = 0f;
    public float secondsPerSpawn = 1f;

    private int spawnCount = 0;


    void Start() {}

    void Update() {
        Instantiate(spawnObject, spawnPointTransform.position, Quaternion.identity);
        spawnCount += 1;
        Debug.Log("Spawn count: " + spawnCount.ToString());
    }

    void OnGUI() {
        GUI.Label(
            new Rect(0, 0, 100, 100), 
            (1.0f / Time.smoothDeltaTime).ToString("#.00")
        );
    }


}
