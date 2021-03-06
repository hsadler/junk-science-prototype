using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEInletScript : MonoBehaviour
{

    // Spawner for continuous-flow science-element object creation


    public GameObject spawnObject;
    public string scienceElementTag = "science-element-none";
    public float spawnObjectScale = 1f;

    public Transform spawnPointTransform;
    public bool spawnOn = false;
    public int spawnRowLength = 10;
    public int spawnColumnLength = 10;
    public float itemSpread = 1;
    public float propulsionForce = 0f;
    public float secondsPerSpawn = 1f;
    public int spawnPoolSize = 10000;

    private Stack<GameObject> spawnPool;


    // UNITY HOOKS

    void Start() {
        this.spawnPool = new Stack<GameObject>();
        this.FillSpawnPool();
        InvokeRepeating("CheckAndSpawnObjects", 0f, this.secondsPerSpawn);
    }

    void Update() {}

    // IMPLEMENTATION METHODS

    private void FillSpawnPool() {
        for (int i = 0; i < this.spawnPoolSize; i++) {
            GameObject go = Instantiate(
                this.spawnObject, 
                this.spawnPointTransform.position, 
                Quaternion.identity
            ) as GameObject;
            go.SetActive(false);
            go.GetComponent<ScienceElementScript>().elementScale = this.spawnObjectScale;
            go.transform.parent = this.spawnPointTransform;
            go.tag = this.scienceElementTag;
            this.spawnPool.Push(go);
        }
    }

    private void CheckAndSpawnObjects() {
        if(this.spawnOn) {
            for (int i = 0; i < this.spawnColumnLength; i++) {
                for (int j = 0; j < this.spawnRowLength; j++) {
                    if (this.spawnPool.Count < 1) {
                        return;
                    }
                    //Debug.Log("spawnging from inlet");
                    GameObject go = this.spawnPool.Pop();
                    Vector3 localPos = go.transform.localPosition;
                    // TODO: itemSpread is buggy here
                    Vector3 newlocalPos = new Vector3(
                        (localPos.x + (i * this.itemSpread)) - (this.spawnColumnLength / 2),
                        localPos.y,
                        (localPos.z + (j * this.itemSpread)) - (this.spawnRowLength / 2)
                    );
                    go.transform.localPosition = newlocalPos;
                    go.transform.localRotation = Quaternion.identity;
                    go.SetActive(true);
                    go.GetComponent<Rigidbody>().AddRelativeForce(
                        Vector3.down * this.propulsionForce
                    );
                    LabSceneManager.instance.scienceElementSpawnCount += 1;
                }
            }
        }
    }


}
