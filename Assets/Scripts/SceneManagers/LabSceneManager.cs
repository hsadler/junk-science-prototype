using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LabSceneManager : MonoBehaviour
{


    // manager components
    public ScienceElementData scienceElementData = new ScienceElementData();

    // player
    public bool playerActive = false;
    public UnityEvent playerSetActive;
    public UnityEvent playerSetInactive;

    // scene gravity
    public enum GravityEnum
    {
        ninePointEight,
        seventy
    };
    public GravityEnum sceneGravity = GravityEnum.ninePointEight;

    // scene telemetry
    public int scienceElementSpawnCount = 0;
    private Rect guiRect = new Rect(10, 10, 210, 110);

    // science element game object pool
    public GameObject spawnObject;
    public int spawnPoolSize = 10000;
    private Stack<GameObject> scienceElementPool = new Stack<GameObject>();

    // unity events
    public ScienceElementDiscoveredEvent scienceElementDiscoveredEvent = new ScienceElementDiscoveredEvent();


    // the static reference to the singleton instance
    public static LabSceneManager instance { get; private set; }


    // UNITY HOOKS

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetSceneGravity();
        this.FillSpawnPool();
        this.TogglePlayerActive();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.TogglePlayerActive();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        int fps = (int)(1.0f / Time.smoothDeltaTime);
        string displayText =
            "Spawn count: " + this.scienceElementSpawnCount.ToString() +
            ", FPS: " + fps.ToString();
        GUI.Label(
            this.guiRect,
            displayText
        );
    }

    // INTERFACE METHODS

    public GameObject GetScienceElementFromPool()
    {
        if (this.scienceElementPool.Count > 0)
        {
            var go = this.scienceElementPool.Pop();
            this.scienceElementSpawnCount += 1;
            return go;
        }
        else
        {
            return null;
        }
    }

    public void GiveScienceElementBackToPool(GameObject go)
    {
        go.SetActive(false);
        go.GetComponent<Rigidbody>().velocity = Vector3.zero;
        var seScript = go.GetComponent<ScienceElementScript>();
        seScript.InitializeTemperatureProperties();
        this.scienceElementPool.Push(go);
        this.scienceElementSpawnCount -= 1;
    }

    // IMPLEMENTATION METHODS

    private void SetSceneGravity()
    {
        float grav = 0f;
        if (this.sceneGravity == GravityEnum.ninePointEight)
        {
            grav = -9.8f;
        }
        else if (this.sceneGravity == GravityEnum.seventy)
        {
            grav = -70f;
        }
        Physics.gravity = new Vector3(0, grav, 0);
    }

    private void TogglePlayerActive()
    {
        this.playerActive = !this.playerActive;
        if (this.playerActive)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            this.playerSetActive.Invoke();
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            this.playerSetInactive.Invoke();
        }
    }

    private void FillSpawnPool()
    {
        for (int i = 0; i < this.spawnPoolSize; i++)
        {
            GameObject go = Instantiate(
                this.spawnObject,
                transform.position,
                Quaternion.identity
            ) as GameObject;
            go.SetActive(false);
            this.scienceElementPool.Push(go);
        }
    }


}
