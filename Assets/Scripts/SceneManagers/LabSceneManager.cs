using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LabSceneManager : MonoBehaviour
{


	// player
	public bool playerActive = true;
	public UnityEvent playerSetActive;
	public UnityEvent playerSetInactive;

	// scene telemetry
	public int scienceElementSpawnCount = 0;
	private Rect guiRect = new Rect(10, 10, 210, 110);

	// science element game object pool
	public GameObject spawnObject;
	public int spawnPoolSize = 10000;
	private Stack<GameObject> scienceElementPool = new Stack<GameObject>();


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
		this.FillSpawnPool();
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
		var go = this.scienceElementPool.Pop();
		if (go != null)
		{
			this.scienceElementSpawnCount += 1;
		}
		return go;
	}

	public void GiveScienceElementBackToPool(GameObject go)
    {
		go.GetComponent<Rigidbody>().velocity = Vector3.zero;
		this.scienceElementPool.Push(go);
		this.scienceElementSpawnCount -= 1;
	}

	// IMPLEMENTATION METHODS

	private void TogglePlayerActive()
    {
		this.playerActive = !this.playerActive;
		if (this.playerActive)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			this.playerSetActive.Invoke();
		} else
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

	private void LogPerformance()
	{
		float fps = 1.0f / Time.smoothDeltaTime;
		// fire logs if under fps threshold
		if (fps < 35f)
		{
			Debug.Log("Spawn count: " + this.scienceElementSpawnCount.ToString() + ", FPS: " + fps.ToString("#.00"));
		}
	}

}
