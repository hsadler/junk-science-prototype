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
    public int spawnPoolSize = 10000;

    private int spawnCount = 0;
    private Stack<GameObject> spawnPool;
    private Rect guiRect = new Rect(0, 0, 100, 100);


    void Start() {
        this.spawnPool = new Stack<GameObject>();
        this.FillSpawnPool();
        Debug.Log(spawnPool.ToString());
    }

    void Update() {
        GameObject go = spawnPool.Pop();
        go.SetActive(true);
        this.spawnCount += 1;
        float fps = 1.0f / Time.smoothDeltaTime;
        if(fps < 35f) {
            Debug.Log("Spawn count: " + this.spawnCount.ToString() + ", Fps: " + fps.ToString("#.00"));
        }
    }

    void OnGUI() {
        float fps = 1.0f / Time.smoothDeltaTime;
        string displayText = "Spawn count: " + this.spawnCount.ToString() + ", Fps: " + fps.ToString("#.00");
        GUI.Label(
            this.guiRect, 
            displayText
        );
    }

    private void FillSpawnPool() {
        for (int i = 0; i < this.spawnPoolSize; i++) {
            GameObject go = Instantiate(
                this.spawnObject, 
                this.spawnPointTransform.position, 
                Quaternion.identity
            ) as GameObject;
            this.spawnPool.Push(go);
            go.SetActive(false);
        }
    }


}
