using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEInletScript : MonoBehaviour
{

    // Spawner for continuous-flow science-element object creation


    public bool spawnOn = false;
    public string scienceElementTag = "science-element-none";
    public Transform spawnPointTransform;
    public int spawnRowLength = 10;
    public int spawnColumnLength = 10;
    public float itemSpread = 1;
    public float propulsionForce = 0f;
    public float secondsPerSpawn = 1f;


    // UNITY HOOKS

    void Start() {
        InvokeRepeating("CheckAndSpawnObjects", 0f, this.secondsPerSpawn);
    }

    void Update() { }

    // IMPLEMENTATION METHODS

    private void CheckAndSpawnObjects() {
        if (this.spawnOn) {
            for (int i = 0; i < this.spawnColumnLength; i++) {
                for (int j = 0; j < this.spawnRowLength; j++) {
                    var go = LabSceneManager.instance.GetScienceElementFromPool();
                    if (go == null) {
                        return;
                    }
                    ScienceElementScript seScript = go.GetComponent<ScienceElementScript>();
                    go.transform.parent = this.spawnPointTransform;
                    go.transform.localPosition = Vector3.zero;
                    go.tag = this.scienceElementTag;
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
                }
            }
        }
    }

    public void TurnOff()
    {
        this.spawnOn = false;
    }

    public void TurnOn(
        string scienceElementTag,
        int spawnRowLength = 1,
        int spawnColumnLength = 1,
        float itemSpread = 1f,
        float propulsionForce = 0f,
        float secondsPerSpawn = 1f
    )
    {
        this.scienceElementTag = scienceElementTag;
        this.spawnRowLength = spawnRowLength;
        this.spawnColumnLength = spawnColumnLength;
        this.itemSpread = itemSpread;
        this.propulsionForce = propulsionForce;
        this.secondsPerSpawn = secondsPerSpawn;
        this.spawnOn = true;
    }


}
