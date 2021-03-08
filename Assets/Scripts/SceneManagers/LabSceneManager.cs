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
