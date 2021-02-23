using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InletScript : MonoBehaviour
{

    // Spawner for continuous-flow object creation


    public GameObject spawnObject;
    public Transform spawnDumpTransform;
    public Transform spawnPointTransform;
    public bool spawnOn = false;
    public int spawnRowLength = 10;
    public int spawnColumnLength = 10;
    public float itemSpread = 1;
    public float propulsionForce = 0f;
    public float secondsPerSpawn = 1f;
    public int spawnPoolSize = 10000;

    private int spawnCount = 0;
    private Stack<GameObject> spawnPool;
    private Rect guiRect = new Rect(10, 10, 210, 110);


    void Start() {
        this.spawnPool = new Stack<GameObject>();
        this.FillSpawnPool();
        InvokeRepeating("CheckAndSpawnObjects", 0f, this.secondsPerSpawn);
    }

    void Update() {}

    void OnGUI() {
        int fps = (int)(1.0f / Time.smoothDeltaTime);
        string displayText = "Spawn count: " + this.spawnCount.ToString() + 
            ", Fps: " + fps.ToString();
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
            go.SetActive(false);
            go.transform.parent = this.spawnDumpTransform;
            this.spawnPool.Push(go);
        }
    }

    private void CheckAndSpawnObjects() {
        if(this.spawnOn) {
            foreach (GameObject go in this.GetGameObjectsToSpawn()) {
                if(go != null) {
                    go.SetActive(true);
                    this.spawnCount += 1;
                    
                }
            }
            float fps = 1.0f / Time.smoothDeltaTime;
            // fire logs if under fps threshold
            if(fps < 35f) {
                Debug.Log("Spawn count: " + this.spawnCount.ToString() + 
                    ", Fps: " + fps.ToString("#.00"));
            }
        }
    }

    private List<GameObject> GetGameObjectsToSpawn() {
        var toSpawn = new List<GameObject>();
        for(int i = 0; i < this.spawnColumnLength; i++) {
            for(int j = 0; j < this.spawnRowLength; j++) {
                GameObject go = this.spawnPool.Pop();
                Vector3 localPos = go.transform.localPosition;
                Vector3 newlocalPos = new Vector3(
                    (localPos.x + (i * this.itemSpread)) - (this.spawnColumnLength / 2),
                    localPos.y,
                    (localPos.z + (j * this.itemSpread)) - (this.spawnRowLength / 2)
                );
                go.transform.localPosition = newlocalPos;
                toSpawn.Add(go);
            }    
        }
        return toSpawn;
    }


}
